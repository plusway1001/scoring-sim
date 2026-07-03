using UnityEngine.UIElements;

namespace VideoScope.Pages
{
    public class LoginPageController
    {
        private readonly VisualElement _root;
        private readonly UIManager _manager;

        private Button _tabLogin, _tabSignup, _btnSubmit, _btnGuest;
        private TextField _username, _password;
        private Label _error, _usernameHint;

        private string _mode = "login"; // "login" | "signup"

        public LoginPageController(VisualElement root, UIManager manager)
        {
            _root = root;
            _manager = manager;
        }

        public void Bind()
        {
            _tabLogin = _root.Q<Button>("tab-login");
            _tabSignup = _root.Q<Button>("tab-signup");
            _btnSubmit = _root.Q<Button>("btn-submit");
            _btnGuest = _root.Q<Button>("btn-guest");
            _username = _root.Q<TextField>("input-username");
            _password = _root.Q<TextField>("input-password");
            _error = _root.Q<Label>("error-label");
            _usernameHint = _root.Q<Label>("username-hint");

            _tabLogin.clicked += () => SetMode("login");
            _tabSignup.clicked += () => SetMode("signup");
            _btnSubmit.clicked += Submit;
            _btnGuest.clicked += () => _manager.OnGuest();

            SetMode("login");
        }

        private void SetMode(string mode)
        {
            _mode = mode;
            _tabLogin.EnableInClassList("auth-tab--active", mode == "login");
            _tabSignup.EnableInClassList("auth-tab--active", mode == "signup");
            _btnSubmit.text = mode == "login" ? "Sign In" : "Create Account & Setup Profile";
            _usernameHint.text = mode == "login" ? "Try: GamerSG" : "Choose a username";
            ClearError();
        }

        private void Submit()
        {
            if (_mode == "login") HandleLogin();
            else HandleSignup();
        }

        private void HandleLogin()
        {
            var name = _username.value?.Trim();
            if (string.IsNullOrEmpty(name)) { ShowError("Enter a username"); return; }

            var demo = DemoUsers.Find(name);
            if (demo != null) { _manager.OnLoginSuccess(demo); return; }

            ShowError("User not found. Try 'GamerSG' or 'CasualPlayer', or create an account.");
        }

        private void HandleSignup()
        {
            var name = _username.value?.Trim() ?? "";
            if (name.Length < 3) { ShowError("Username must be 3+ characters"); return; }

            var user = new UserProfile { Username = name };
            _manager.OnLoginSuccess(user);
        }

        private void ShowError(string message)
        {
            _error.text = message;
            _error.RemoveFromClassList("hidden");
        }

        private void ClearError()
        {
            _error.text = "";
            _error.AddToClassList("hidden");
        }
    }
}
