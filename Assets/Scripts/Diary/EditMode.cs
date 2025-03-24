using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiClient;
using Dto;
using UnityEngine;
using Util;

namespace Diary
{
    public class EditMode : DiaryMode
    {
        public EditMode(DiaryWriterManager diaryWriter) : base(diaryWriter) { }

        public override void Setup()
        {
            _diaryWriter.GetContentFieldText().interactable = true;
            _diaryWriter.buttonSave.gameObject.SetActive(true);
            _diaryWriter.ClearText.gameObject.SetActive(true);
            _diaryWriter.buttonSwitchToEditMode.gameObject.SetActive(false);
        }

        public async override void HandleSave()
        {

            string _diaryContent = _diaryWriter.GetContentFieldText().text;

            if (_diaryWriter.GetIsExistend())
            {
                PutSpecificDiaryContentRequestDto putContentDto = new PutSpecificDiaryContentRequestDto { date = _diaryWriter.GetDiaryDate(), content = _diaryContent };
                bool response = await DiaryApiClient.PutSpecificDiaryContent(putContentDto);
            }
            else
            {
                PostSpecificDiaryContentRequestDto postContentDto = new PostSpecificDiaryContentRequestDto { date = _diaryWriter.GetDiaryDate(), content = _diaryContent };
                bool response = await DiaryApiClient.PostSpecificDiaryContent(postContentDto);
            }
        }
        

        public override void HandleGoBack()
        {
            if (_diaryWriter.GetDiaryContent() != _diaryWriter.GetContentFieldText().text)
            {
                Confirmations.CreateConfirmationPopup(_diaryWriter.GetConfirmPopupGoBack(), _diaryWriter.GetSaveAllFirstBeforeGoBack(), "nog niet alles is opgeslagen, wil je het nog opslaan?", _diaryWriter.GetComponent<CanvasGroup>());


                //ConfirmPopup.gameObject.SetActive(true);
            }
            else
            {
                _diaryWriter.GetConfirmPopupGoBack().Invoke();
            }

        }

        public override void HandleClose()
        {
            if (_diaryWriter.GetDiaryContent() != _diaryWriter.GetContentFieldText().text)
            {
                Confirmations.CreateConfirmationPopup(_diaryWriter.GetConfirmPopupClose(), _diaryWriter.GetSaveAllFirstBeforeClose(), "nog niet alles is opgeslagen, wil je het nog opslaan?", _diaryWriter.GetComponent<CanvasGroup>());


                //ConfirmPopup.gameObject.SetActive(true);
            }
            else
            {
                _diaryWriter.GetConfirmPopupClose().Invoke();
            }
        }
    }
}
