namespace Code.CopyEngine.Core.Notification
{
    public static class CEUID
    {
        private static int mNowID = 1;

        public static int NewOne()
        {
            mNowID++;
            return mNowID;
        }
    }
}