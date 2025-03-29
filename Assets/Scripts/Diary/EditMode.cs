using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiClient;
using Dto;
using UnityEngine;
using Util;
using Global;

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

            string diaryContent = _diaryWriter.GetContentFieldText().text;
            _diaryWriter.SetDiaryContent(diaryContent);

            if (_diaryWriter.GetIsExistend())
            {
                PutDiaryContentRequestDto putContentDto = new PutDiaryContentRequestDto { content = diaryContent };
                Debug.Log("de put content dto is: " +putContentDto.content + " " + _diaryWriter.GetDiaryDate());
                bool response = await DiaryApiClient.PutDiaryContent(putContentDto, _diaryWriter.GetDiaryDate());
                Debug.Log("we hebben een put gedaan en  de response is: " + response);

                if (response){
                    DiarySingleton.Instance.UpdateDiaryData(new DiaryReadDto { date = _diaryWriter.GetDiaryDate().Date, content = diaryContent });
                }
            }
            else
            {
                PostDiaryContentRequestDto postContentDto = new PostDiaryContentRequestDto { date = _diaryWriter.GetDiaryDate().Date, content = diaryContent };
                Debug.Log("de post content dto is: " + postContentDto.date + " " + postContentDto.content);
                bool response = await DiaryApiClient.PostDiaryContent(postContentDto);
                Debug.Log("we hebben een post gedaan en  de response is: " + response);
                if (response)
                {
                    _diaryWriter.SetIsExistend(true);
                    DiarySingleton.Instance.AddDiaryData(new DiaryReadDto { date = _diaryWriter.GetDiaryDate().Date, content = diaryContent });
                }
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
