// =====JANE SECTION=====
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace GameScope
{
    public class ThemedDropdown : VisualElement, INotifyValueChanged<string>
    {
        // Only one ThemedDropdown's menu should ever be open at a time across the
        // whole app — opening a new one closes whichever other one was open.
        private static ThemedDropdown _currentlyOpen;

        private readonly Label _valueLabel;
        private readonly ScrollView _menu;
        private readonly EventCallback<ClickEvent> _outsideClickHandler;

        private List<string> _choices = new List<string>();
        private string _value = "";
        private bool _open;

        public List<string> choices
        {
            get => _choices;
            set { _choices = value ?? new List<string>(); RebuildMenu(); }
        }

        public int index
        {
            get => _choices.IndexOf(_value);
            set
            {
                if (value >= 0 && value < _choices.Count)
                    this.value = _choices[value];
            }
        }

        public string value
        {
            get => _value;
            set
            {
                if (_value == value) return;
                string previous = _value;
                SetValueWithoutNotify(value);
                using (var evt = ChangeEvent<string>.GetPooled(previous, _value))
                {
                    evt.target = this;
                    SendEvent(evt);
                }
            }
        }

        public void SetValueWithoutNotify(string newValue)
        {
            _value = newValue;
            _valueLabel.text = _value;
            RefreshSelectedRow();
        }

        public ThemedDropdown()
        {
            AddToClassList("themed-dropdown");
            pickingMode = PickingMode.Position;

            var trigger = new VisualElement();
            trigger.AddToClassList("themed-dropdown__trigger");
            _valueLabel = new Label();
            _valueLabel.AddToClassList("themed-dropdown__label");
            var arrow = new VisualElement();
            arrow.AddToClassList("themed-dropdown__arrow");
            trigger.Add(_valueLabel);
            trigger.Add(arrow);
            Add(trigger);

            _menu = new ScrollView();
            _menu.AddToClassList("themed-dropdown__menu");
            _menu.style.display = DisplayStyle.None;
            Add(_menu);

            _outsideClickHandler = OnOutsideClick;

            trigger.RegisterCallback<ClickEvent>(evt =>
            {
                evt.StopPropagation();
                if (_open) CloseMenu(); else OpenMenu();
            });
        }

        private void OpenMenu()
        {
            if (_currentlyOpen != null && _currentlyOpen != this)
                _currentlyOpen.CloseMenu();
            _currentlyOpen = this;

            _open = true;
            AddToClassList("themed-dropdown--open");
            var pageRoot = FindPageRoot();
            if (pageRoot != null)
            {
                var worldRect = worldBound;
                var localTopLeft = pageRoot.WorldToLocal(new UnityEngine.Vector2(worldRect.x, worldRect.yMax + 4));
                _menu.style.position = Position.Absolute;
                _menu.style.left = localTopLeft.x;
                _menu.style.top = localTopLeft.y;
                _menu.style.width = worldRect.width;
                pageRoot.Add(_menu);
            }

            _menu.style.display = DisplayStyle.Flex;
            panel?.visualTree.RegisterCallback(_outsideClickHandler);
        }

        private void CloseMenu()
        {
            if (_currentlyOpen == this) _currentlyOpen = null;

            _open = false;
            _menu.style.display = DisplayStyle.None;
            RemoveFromClassList("themed-dropdown--open");
            panel?.visualTree.UnregisterCallback(_outsideClickHandler);
            if (_menu.parent != this)
            {
                _menu.RemoveFromHierarchy();
                Add(_menu);
            }
        }

        private VisualElement FindPageRoot()
        {
            var el = parent;
            while (el != null)
            {
                if (el.ClassListContains("page")) return el;
                el = el.parent;
            }
            return panel?.visualTree;
        }

        private void OnOutsideClick(ClickEvent evt)
        {
            var target = evt.target as VisualElement;
            if (target == null || !Contains(target))
                CloseMenu();
        }

        private void RebuildMenu()
        {
            _menu.Clear();
            foreach (var choice in _choices)
            {
                string c = choice;
                var row = new VisualElement();
                row.AddToClassList("themed-dropdown__option");
                var text = new Label(c);
                text.AddToClassList("themed-dropdown__option-text");
                row.Add(text);
                row.RegisterCallback<ClickEvent>(evt =>
                {
                    evt.StopPropagation();
                    value = c;
                    CloseMenu();
                });
                _menu.Add(row);
            }
            RefreshSelectedRow();
        }

        private void RefreshSelectedRow()
        {
            foreach (var row in _menu.Children())
            {
                var text = row.Q<Label>(className: "themed-dropdown__option-text");
                if (text == null) continue;
                row.EnableInClassList("themed-dropdown__option--selected", text.text == _value);
            }
        }
    }
}

// =====END OF JANE SECTION=====
