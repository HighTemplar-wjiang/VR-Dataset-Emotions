from django.urls import path

from . import views

urlpatterns = [
    path("", views.index, name="index_page"),
    path("login/", views.login, name="participant_login"),
    path("register/", views.register, name="participant_register"),
    path("emotion-survey/", views.emotion_survey, name="emotion_survey"),
    path("camera-pose/", views.camera_pose, name="camera_pose"),
    path("finish-study/", views.finish_study, name="finish_study"),
    path("finish-study/status/", views.check_study_finished, name="check_study_finished"),
]
