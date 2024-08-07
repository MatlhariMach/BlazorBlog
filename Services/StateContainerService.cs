namespace BlazorBlog.Services
{
    public class StateContainerService
    {
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                NotifyStateChanged();
            }
        }

        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }

    /*  public class StateContainerService
      {
          private string _state;

          public string State
          {
              get => _state;
              set
              {
                  _state = value;
                  NotifyStateChanged();
              }
          }

          public event Action OnStateChange;

          private void NotifyStateChanged() => OnStateChange?.Invoke();
      } */
}

