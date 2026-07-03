using UnityEngine;
using UnityEngine.UIElements;
using VideoScope.Pages;

namespace VideoScope
{
    /// <summary>
    /// Attach to a GameObject that has a UIDocument component. Assign the six
    /// VisualTreeAssets below in the Inspector. This drives all navigation and
    /// holds the shared app state (current user, currently viewed game) that
    /// the React version kept in the root <App/> component's useState hooks.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public class UIManager : MonoBehaviour
    {
        [Header("Page templates")]
        public VisualTreeAsset loginPage;
        public VisualTreeAsset onboardingPage;
        public VisualTreeAsset cataloguePage;
        public VisualTreeAsset gameDetailPage;
        public VisualTreeAsset profilePage;
        public VisualTreeAsset gameCardTemplate;

        private VisualElement _root;
        private UIDocument _document;

        // ---- App-wide state (equivalent to App()'s useState hooks) ----
        public UserProfile CurrentUser { get; private set; }
        public GameEntry SelectedGame { get; private set; }
        public bool IsNewUser { get; private set; }

        private void OnEnable()
        {
            _document = GetComponent<UIDocument>();
            _root = _document.rootVisualElement;

            // The root panel can be any actual window size; every page is authored
            // at a fixed 1920x1080 canvas (see .page in VideoScope.uss), so center
            // that canvas within whatever space the panel gives us.
            _root.style.flexGrow = 1;
            _root.style.alignItems = Align.Center;
            _root.style.justifyContent = Justify.Center;
            _root.style.backgroundColor = new Color(0f, 0f, 0f);

            ShowLogin();
        }

        private VisualElement SwapTo(VisualTreeAsset asset)
        {
            _root.Clear();
            var instance = asset.CloneTree();
            _root.Add(instance);
            return instance;
        }

        // ---- Navigation, mirrors handleLogin / handleGuest / handleNav / etc. ----

        public void ShowLogin()
        {
            CurrentUser = null;
            var pageRoot = SwapTo(loginPage);
            new LoginPageController(pageRoot, this).Bind();
        }

        public void OnLoginSuccess(UserProfile user)
        {
            CurrentUser = user;
            if (!user.HasOnboarded)
            {
                IsNewUser = true;
                ShowOnboarding();
            }
            else
            {
                ShowCatalogue();
            }
        }

        public void OnGuest()
        {
            CurrentUser = null;
            ShowCatalogue();
        }

        public void ShowOnboarding()
        {
            var pageRoot = SwapTo(onboardingPage);
            new OnboardingPageController(pageRoot, this).Bind();
        }

        public void OnOnboardingComplete(UserProfile user)
        {
            CurrentUser = user;
            IsNewUser = false;
            ShowCatalogue();
        }

        public void ShowCatalogue()
        {
            SelectedGame = null;
            var pageRoot = SwapTo(cataloguePage);
            new CataloguePageController(pageRoot, this).Bind();
        }

        public void ShowGameDetail(GameEntry game)
        {
            SelectedGame = game;
            var pageRoot = SwapTo(gameDetailPage);
            new GameDetailPageController(pageRoot, this, game).Bind();
        }

        public void ShowProfile()
        {
            if (CurrentUser == null) { ShowLogin(); return; }
            var pageRoot = SwapTo(profilePage);
            new ProfilePageController(pageRoot, this).Bind();
        }

        public void SignOut()
        {
            CurrentUser = null;
            ShowLogin();
        }

        /// <summary>Call after mutating CurrentUser (wishlist/rating/etc.) from a detail
        /// or profile view when you want to return to the catalogue afterwards.</summary>
        public void SetUser(UserProfile user) => CurrentUser = user;
    }
}
