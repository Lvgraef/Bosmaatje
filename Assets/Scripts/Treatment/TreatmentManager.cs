using System;
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
        public GameObject dateSelectButton;
        public Image treatmentImage;
        public GameObject previousButton;
        public GameObject nextButton;
        public DatePicker treatmentDate;
        private int _currentPage;
        private bool _isCompleted;
        private string[] _description;
        private string _videoPath;
        [CanBeNull] private string _stickerId;
        private bool _canEdit;

        public void Initialize(string treatmentName, string[] description, string imagePath, string videoPath, DateTime? date, [CanBeNull] string stickerId)
        {
            dateSelectButton.SetActive(false);
            _videoPath = videoPath;
            if (date != null)
            {
                treatmentDate.SelectedDate = new SerializableDate { Date = date.Value };
            }
            this.treatmentName.text = treatmentName;
            _stickerId = stickerId;
            treatmentImage.sprite = Resources.Load<Sprite>( imagePath);
            _description = description;
            _currentPage = 0;
            UpdateDescription();
        }

        public void UpdateDescription()
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

        public void EditTreatment()
        {
            _canEdit = !_canEdit;
        }

        public void PutSticker()
        {
            throw new NotImplementedException();
        }

        public void ShowVideo()
        {
            throw new NotImplementedException();
        }
    }
}