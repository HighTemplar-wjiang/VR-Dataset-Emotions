from django.db import models


# Create your models here.
class Participant(models.Model):
    """
    Participant table.
    """
    # Participant metadata.
    participant_id = models.AutoField(primary_key=True)
    prolific_id = models.CharField(max_length=200, unique=False)
    login_id = models.CharField(max_length=200)

    # Action logs.
    register_time = models.DateTimeField("time registered")
    last_login_time = models.DateTimeField("last login time", null=True, blank=True)
    finish_time = models.DateTimeField("study finish time", null=True, blank=True)

    # Study settings.
    scene_sequence = models.CharField(max_length=1000, unique=False, null=False, blank=False)

    # Flags.
    study_finished = models.BooleanField(default=False)

    def __str__(self):
        return (f"PID: {self.participant_id}, "
                f"Prolific ID: {self.prolific_id}, "
                f"Login ID: {self.login_id}, "
                f"Registered: {str(self.register_time)}, "
                f"Last login: {str(self.last_login_time)}, "
                f"Finish time: {str(self.finish_time)}, "
                f"Study finished: {str(self.study_finished)}")


class EmotionSurvey(models.Model):
    """
    Record emotion responses.
    """
    # Metadata.
    login_id = models.CharField(max_length=200, unique=False)  # No external refer, will post filter
    participant_id = models.CharField(max_length=200, unique=False)  # No external refer, will post filter
    vr_timestamp = models.CharField(max_length=100, null=False)
    server_timestamp = models.DateTimeField("emotion survey record time")

    # Survey data.
    # Use char to avoid any type conversion, will post-process.
    scene_name = models.CharField(max_length=100, null=True, blank=True)
    valence_value = models.CharField(max_length=100, null=True, blank=True)
    arousal_value = models.CharField(max_length=100, null=True, blank=True)
    dominance_value = models.CharField(max_length=100, null=True, blank=True)

    def __str__(self):
        return (f"PID: {self.participant_id}, "
                f"Login ID: {self.login_id},"
                f"Server_time: {str(self.server_timestamp)}, "
                f"VR_time: {str(self.vr_timestamp)}, "
                f"Scene: {self.scene_name}, "
                f"Valence: {self.valence_value}, "
                f"Arousal: {self.arousal_value}, "
                f"Dominance: {self.dominance_value}")


class CameraPose(models.Model):
    """
    Record camera pose in time, including position and orientation.
    """
    # Metadata.
    login_id = models.CharField(max_length=200, unique=False)  # No external refer, will post filter
    participant_id = models.CharField(max_length=200, unique=False)  # No external refer, will post filter
    vr_timestamp = models.CharField(max_length=100, null=True, blank=True)  # Timestamp in VR.
    server_timestamp = models.DateTimeField("camera pose record time in server", null=True, blank=True)

    # Scene name.
    scene_name = models.CharField(max_length=100)

    # Position.
    position = models.CharField(max_length=100)

    # Orientation.
    # Could be Euler angles or quaternions.
    orientation = models.CharField(max_length=100)

    # String representation.
    def __str__(self):
        return (f"PID: {self.participant_id}, "
                f"Login ID: {self.login_id},"
                f"Server_time: {str(self.server_timestamp)}, "
                f"VR_time: {str(self.vr_timestamp)}, "
                f"Scene: {self.scene_name}, "
                f"Position: {self.position}, "
                f"Orientation: {self.orientation}")
