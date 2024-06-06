import json
import urllib
import time
from datetime import datetime
from django.utils import timezone

from django.shortcuts import render
from django.http import HttpResponse, HttpResponseBadRequest, JsonResponse
from django.views.decorators.csrf import csrf_exempt
from .models import Participant, EmotionSurvey, CameraPose
from .utils import *


# Index page, including
# Title of the study.
# Description of the study.
# Link to the study.
def index(request):
    return render(request, "index.html")


# Participant login.
@csrf_exempt
def login(request):
    # Read post data.
    login_data = json.loads(request.body)

    # Check login data.
    try:
        login_id = login_data["loginId"]
    except Exception as e:
        # Key error.
        print(e)
        return HttpResponseBadRequest("Wrong JSON format.")

    # Check if participant exists by login ID.
    try:
        participant = Participant.objects.get(login_id=login_id)
        # If multiple participants found, return the one with the latest register time.
        if isinstance(participant, list):
            participant = participant[0]

    except Exception as e:
        # Participant not found.
        print(e)
        # Prepare JSON for response.
        response_data = {"result": False,
                         "participantId": "-1",
                         "message": "PIN code unmatched. Forgot your PIN code? You can register again in Prolific."}
        # Return response.
        return HttpResponse(json.dumps(response_data), content_type="application/json")

    # Update last login time.
    participant.last_login_time = timezone.now()
    participant.save()

    # Check if participant has finished the study.
    if participant.study_finished:
        # Prepare JSON for response.
        response_data = {"result": False,
                         "participantId": participant.participant_id,
                         "message": "You have already finished the study."}
        # Return response.
        return HttpResponse(json.dumps(response_data), content_type="application/json")

    # Prepare JSON for response.
    # Get the scene sequence, add Tutorial Scene at the beginning.
    scene_sequence = ["Tutorial"] + eval(participant.scene_sequence)
    response_data = {"result": True,
                     "participantId": participant.participant_id,
                     "message": "Login success.",
                     "sequence": scene_sequence}

    # Return response.
    return HttpResponse(json.dumps(response_data), content_type="application/json")


# Participant register.
@csrf_exempt
def register(request):

    # Read post data.
    try:
        register_data = json.loads(request.body)
    except Exception as e:
        # JSON format error.
        print(e)
        # Prepare JSON including 1) participant_id as -1 and 2) result_message showing the error message.
        response_data = {"participant_id": -1,
                         "result_message": "Wrong JSON format."}
        # Return response.
        return HttpResponse(json.dumps(response_data), content_type="application/json")

    # Check keys.
    try:
        prolific_id = register_data["prolific_id"]
        pincode = register_data["pincode"]
    except Exception as e:
        # Key error.
        print(e)
        # Prepare JSON including 1) participant_id as -1 and 2) result_message showing the error message.
        response_data = {"participant_id": -1,
                         "result_message": "Wrong JSON format."}
        # Return response.
        return HttpResponse(json.dumps(response_data), content_type="application/json")

    # Create a new participant.
    try:
        new_participant = Participant(prolific_id=prolific_id,
                                      login_id=pincode,
                                      register_time=timezone.now(),
                                      last_login_time=timezone.now(),
                                      finish_time=None,
                                      scene_sequence=str(generate_shuffled_scenes()))
        new_participant.save()
    except Exception as e:
        print(e)
        # Return participant_id as -1.
        # Prepare JSON including 1) participant_id as -1 and 2) result_message showing the error message.
        response_data = {"participant_id": -1,
                         "result_message": "Participant creation error."}
        # Return response.
        return HttpResponse(json.dumps(response_data), content_type="application/json")

    # Prepare JSON for response.
    response_data = {"participant_id": new_participant.participant_id}

    # Return response.
    return HttpResponse(json.dumps(response_data), content_type="application/json")


# Post emotion survey result.
@csrf_exempt
def emotion_survey(request):

    # Read post data to JSON.
    try:
        survey_data = json.loads(request.body)
        print(survey_data)
    except Exception as e:
        # JSON format error.
        print(e)
        # Prepare JSON including 1) participant_id as -1 and 2) result_message showing the error message.
        response_data = {"participant_id": -1,
                         "result_message": "Wrong JSON format."}
        # Return response.
        return HttpResponse(json.dumps(response_data), content_type="application/json")

    # Check keys.
    try:
        participant_id = survey_data["participantId"]
        login_id = survey_data["loginId"]
        vr_timestamp = survey_data["timestampUtcUnixMs"]
        scene_name = survey_data["sceneName"]
        valence_value = survey_data["valenceValue"]
        arousal_value = survey_data["arousalValue"]
        dominance_value = survey_data["dominanceValue"]
    except Exception as e:
        # Key error.
        print(e)
        # Prepare JSON including 1) participant_id as -1 and 2) result_message showing the error message.
        response_data = {"participant_id": -1,
                         "result_message": "Wrong JSON format."}
        # Return response.
        return HttpResponse(json.dumps(response_data), content_type="application/json")

    # Record the survey data.
    try:
        # Create survey emotion object.
        new_survey = EmotionSurvey(participant_id=participant_id,
                                   login_id=login_id,
                                   vr_timestamp=vr_timestamp,
                                   server_timestamp=timezone.now(),
                                   scene_name=scene_name,
                                   valence_value=valence_value,
                                   arousal_value=arousal_value,
                                   dominance_value=dominance_value)
        new_survey.save()
    except Exception as e:
        print(e)
        # Prepare JSON including 1) participant_id as -1 and 2) result_message showing the error message.
        response_data = {"participant_id": -1,
                         "result_message": "Survey data recording error."}
        # Return response.
        return HttpResponse(json.dumps(response_data), content_type="application/json")

    # Prepare JSON for response.
    response_data = {"participant_id": participant_id,
                     "result_message": "Survey data recorded."}

    # Return response.
    return HttpResponse(json.dumps(response_data), content_type="application/json")


# Record the camera offset data from VR.
@csrf_exempt
def camera_pose(request):
    if request.method == "POST":
        try:
            # Parse the incoming JSON binary data as binary.
            decoded_data = urllib.parse.unquote(request.body.decode("utf-8"))

            # Save the data to the file.
            # Prepare the filename in full path with Unix timestamp.
            # Create a folder by date if not exist.
            folder_name = "./log/request/" + datetime.now().strftime("%Y-%m-%d")
            request_filename = os.path.join(folder_name, "./camera_pose_data_" + str(int(time.time())) + ".json")
            full_path = os.path.join(os.path.dirname(os.path.abspath(__file__)) + request_filename)
            # Make the folder is not exist.
            if not os.path.exists(os.path.dirname(full_path)):
                os.makedirs(os.path.dirname(full_path))
            # Save the file.
            with open(full_path, "w") as f:
                f.write(decoded_data)

            # Parse the incoming JSON data.
            data = json.loads(decoded_data)

            # Extract key information from the data.
            login_id = data["loginId"]
            participant_id = data["participantId"]
            scene_name = data["sceneName"]
            camera_poses = data["cameraPoses"]

            # Loop through the cameraPoses list and extract individual data.
            for item in camera_poses:
                position = item["position"]  # This will be a dictionary with "x", "y", and "z".
                orientation = item["orientation"]  # This will be a dictionary with "x", "y", "z", and "w".
                timestamp = item["timestamp"]  # This is the timestamp

                try:
                    # Construct data rows and save them to the database.
                    new_camera_pose = CameraPose(participant_id=participant_id,
                                                 login_id=login_id,
                                                 scene_name=scene_name,
                                                 position=str(position),
                                                 orientation=str(orientation),
                                                 vr_timestamp=timestamp,
                                                 server_timestamp=timezone.now())
                    new_camera_pose.save()
                except Exception as e:
                    # Handle errors and send an error response.
                    print(e)
                    return JsonResponse({"status": "error", "message": str(e)})

            # Debug info.
            print("Camera pose data recorded: ", len(camera_poses))

            # Send a success response back.
            return JsonResponse({"status": "success"})
        except Exception as e:
            # Handle errors and send an error response.
            print(e)
            return JsonResponse({"status": "error", "message": str(e)})
    else:
        # Ensure only POST requests are processed.
        print("Only POST requests are allowed.")
        return JsonResponse({"status": "error", "message": "Only POST requests are allowed."})


# Mark participant as finished
@csrf_exempt
def finish_study(request):
    # Read post data.
    try:
        # Decoding https request body.
        decoded_data = urllib.parse.unquote(request.body.decode("utf-8"))
        finish_data = json.loads(decoded_data)
    except Exception as e:
        # JSON format error.
        print(e)
        # Prepare JSON including 1) participant_id as -1 and 2) result_message showing the error message.
        response_data = {"participant_id": -1,
                         "result_message": "Wrong JSON format."}
        # Return response.
        return HttpResponse(json.dumps(response_data), content_type="application/json")

    # Check keys.
    try:
        participant_id = finish_data["participantId"]
    except Exception as e:
        # Key error.
        print(e)
        # Prepare JSON including 1) participant_id as -1 and 2) result_message showing the error message.
        response_data = {"participant_id": -1,
                         "result_message": "Wrong JSON format."}
        # Return response.
        return HttpResponse(json.dumps(response_data), content_type="application/json")

    # Mark the participant as finished.
    try:
        # Get the participant.
        participant = Participant.objects.get(participant_id=participant_id)
        # Mark the finish time.
        participant.finish_time = timezone.now()
        # Mark the participant as finished.
        participant.study_finished = True
        # Save the participant.
        participant.save()
    except Exception as e:
        print(e)
        # Prepare JSON including 1) participant_id as -1 and 2) result_message showing the error message.
        response_data = {"participant_id": -1,
                         "result_message": "Participant finish error."}
        # Return response.
        return HttpResponse(json.dumps(response_data), content_type="application/json")

    # Prepare JSON for response.
    response_data = {"participant_id": participant_id,
                     "result_message": "Participant finished."}

    # Return response.
    return HttpResponse(json.dumps(response_data), content_type="application/json")


# Check if participant finished the study.
@csrf_exempt
def check_study_finished(request):
    # Read post data.
    try:
        # Decoding https request body.
        decoded_data = urllib.parse.unquote(request.body.decode("utf-8"))
        login_data = json.loads(decoded_data)
        print(login_data)
    except Exception as e:
        # JSON format error.
        print(e)
        # Prepare JSON including 1) participant_id as -1 and 2) result_message showing the error message.
        response_data = {"participant_id": -1,
                         "result_message": "Wrong JSON format."}
        # Return response.
        return HttpResponse(json.dumps(response_data), content_type="application/json")

    # Check keys.
    try:
        login_id = login_data["login_id"]
    except Exception as e:
        # Key error.
        print(e)
        # Prepare JSON including 1) participant_id as -1 and 2) result_message showing the error message.
        response_data = {"participant_id": -1,
                         "result_message": "Wrong JSON format."}
        # Return response.
        return HttpResponse(json.dumps(response_data), content_type="application/json")

    # Check if participant exists by login ID.
    try:
        participant = Participant.objects.filter(login_id=login_id).order_by("-last_login_time")
        # Check if empty.
        if len(participant) == 0:
            raise Exception("Participant not found.")
        else:
            participant = participant[0]
    except Exception as e:
        # Participant not found.
        print(e)
        # Prepare JSON for response.
        response_data = {"result": False,
                         "participantId": "-1",
                         "message": "Participant not found."}
        # Return response.
        return HttpResponse(json.dumps(response_data), content_type="application/json")

    # Check if participant has finished the study.
    if participant.study_finished:
        # Prepare JSON for response.
        response_data = {"result": "True",  # This is a string because it will be parsed by Qualtrics.
                         "participantId": participant.participant_id,
                         "message": "You have already finished the study."}
        print(response_data)
        # Return response.
        return HttpResponse(json.dumps(response_data), content_type="application/json")
    else:
        # Prepare JSON for response.
        response_data = {"result": "False",  # This is a string because it will be parsed by Qualtrics.
                         "participantId": participant.participant_id,
                         "message": "You have not finished the study."}
        print(response_data)
        # Return response.
        return HttpResponse(json.dumps(response_data), content_type="application/json")
