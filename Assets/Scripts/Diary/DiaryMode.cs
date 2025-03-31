namespace Diary
{
    public abstract class DiaryMode
    {
        protected DiaryWriterManager _diaryWriter;

        public DiaryMode(DiaryWriterManager diaryWriter)
        {
            _diaryWriter = diaryWriter;
        }

        public abstract void Setup();
        public abstract void HandleSaveUpdater();
        public abstract void HandleGoBack();
        public abstract void HandleClose();

        public abstract void HandleTopBarSwitchMode();

        public abstract void HandleButtomMiddleSwitchMode();

    }

}
