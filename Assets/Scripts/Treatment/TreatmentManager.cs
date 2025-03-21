using System;
using ApiClient;
using Dto;
using JetBrains.Annotations;
using TMPro;
using UI.Dates;
using UnityEngine;
using UnityEngine.UI;

namespace Treatment
{
    public class TreatmentManager : MonoBehaviour
    {
        
        public TextMeshProUGUI treatmentName;
        public TextMeshProUGUI treatmentDescription;
        public TMP_InputField doctorName;
        public GameObject editMode;
        public GameObject dateSelectButton;
        public Image treatmentImage;
        public GameObject previousButton;
        public GameObject nextButton;
        public GameObject dateBlocker;
        public GameObject doctorNameBlocker;
        public GameObject video;
        public DatePicker treatmentDate;
        public GameObject videoButton;
        private int _currentPage;
        private bool _isCompleted;
        private string[] _description;
        [CanBeNull] private string _videoPath;
        [CanBeNull] private string _stickerId;
        private Guid _id;
        private bool _canEdit;

        public void Initialize(Guid id, string treatmentName, string[] description, string imagePath, [CanBeNull] string videoPath, DateTime? date, [CanBeNull] string stickerId, string doctorName)
        {
            _id = id;
            if (videoPath == null)
            {
                videoButton.SetActive(false);
            }
            this.doctorName.text = doctorName;
            dateSelectButton.SetActive(false);
            doctorNameBlocker.SetActive(true);
            dateBlocker.SetActive(true);
            _videoPath = videoPath;
            if (date != null)
            {
                treatmentDate.SelectedDate = new SerializableDate { Date = date.Value };
            }
            this.treatmentName.text = treatmentName;
            _stickerId = stickerId;
            treatmentImage.sprite = Resources.Load<Sprite>(imagePath);
            _description = description;
            _currentPage = 0;
            UpdateDescription();
        }

        private void UpdateDescription()
        {
            treatmentDescription.text = _description[_currentPage];
            previousButton.SetActive(_currentPage > 0);
            nextButton.SetActive(_currentPage < _description.Length - 1);
        }

        public void Close()
        {
            Destroy(gameObject);
        }
        
        public void NextPage()
        {
            _currentPage++;
            UpdateDescription();
        }
        
        public void PreviousPage()
        {
            _currentPage--;
            UpdateDescription();
        }
        
        public void CompleteTreatment()
        {
            _isCompleted = true;
        }

        public async void EditTreatment()
        {
            if (_canEdit)
            {
                await TreatmentPlanApiClient.PutTreatment(_id, new PutTreatmentRequestDto
                {
                    date = treatmentDate.SelectedDate.HasValue ? treatmentDate.SelectedDate.Date : null,
                    doctorName = doctorName.text
                });
            }
            
            _canEdit = !_canEdit;
            dateSelectButton.SetActive(_canEdit);
            doctorNameBlocker.SetActive(!_canEdit);
            dateBlocker.SetActive(!_canEdit);
            editMode.SetActive(_canEdit);
        }

        public void PutSticker()
        {
            throw new NotImplementedException();
        }

        public async void ShowVideo()
        {
            var vid = await InstantiateAsync(video, transform.parent);
            vid[0].GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            vid[0].GetComponent<VideoManager>().SetVideo(_videoPath);
        }
    }
}