using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary
{
    public class PreviewMode : DiaryMode
    {
        public PreviewMode(DiaryWriterManager diaryWriter) : base(diaryWriter) { }

        public override void Setup()
        {
            _diaryWriter.GetContentFieldText().interactable = false;
            _diaryWriter.buttonSave.gameObject.SetActive(false);
            _diaryWriter.ClearText.gameObject.SetActive(false);
            _diaryWriter.buttonSwitchToEditMode.gameObject.SetActive(true);
        }

        public override void HandleSave()
        {
            Debug.WriteLine("Preview mode, cannot save");// save knop zou niet zichtbaar moeten zijn
        }

        public override void HandleGoBack()
        {
            _diaryWriter.GetConfirmPopupGoBack().Invoke();
        }

        public override void HandleClose()
        {
            _diaryWriter.GetConfirmPopupClose().Invoke();
        }
    }

}
