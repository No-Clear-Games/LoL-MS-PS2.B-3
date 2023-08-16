namespace NoClearGames.UI
{
    public class TutorialPopUp : BasePage
    {
        public override void Awake()
        {
            base.Awake();

            backBtn.onClick.AddListener(() => Hide());
        }
    }
}