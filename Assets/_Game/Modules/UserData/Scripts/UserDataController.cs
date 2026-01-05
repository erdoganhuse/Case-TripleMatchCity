using Library.PersistentData;

namespace Modules.UserData
{
    public class UserDataController
    {
        private PersistentVariable<UserData>
            _userData = new($"{nameof(UserDataController)}-{nameof(_userData)}", new());

        public UserDataController() { }

        #region Level Method

        public int GetLevel()
        {
            return _userData.Value.Level;
        }
        
        public void IncrementLevel()
        {
            _userData.Value.Level += 1;
            _userData.ForceSave();
        }

        #endregion
        
        #region Coin Methods

        public int GetCoinAmount()
        {
            return _userData.Value.CurrencyAmount;
        }
        
        public bool CanSpendCoin(int amount)
        {
            return GetCoinAmount() >= amount;
        }

        public void IncreaseCoinAmount(int changeAmount, bool claimInstantly = false)
        {
            _userData.Value.CurrencyAmount += changeAmount;
            _userData.ForceSave();
        }

        public void DecreaseCoinAmount(int changeAmount)
        {
            _userData.Value.CurrencyAmount -= changeAmount;
            _userData.ForceSave();
        }
        
        #endregion
    }
}