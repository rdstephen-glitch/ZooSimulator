namespace ZooSimulator.Api.Models
{
    public class Elephant : Animal
    {
        private const double WalkingThreshold = 70;

        private bool _wasUnableToWalkLastHour;

        public Elephant(string name) : base(name) { }

        public bool CanWalk => IsAlive && Health >= WalkingThreshold;

        protected override void ApplyHealthRules()
        {
            if (Health >= WalkingThreshold)
            {
                _wasUnableToWalkLastHour = false;
            }
            else if (_wasUnableToWalkLastHour)
            {
                Terminate();
            }
            else
            {
                _wasUnableToWalkLastHour = true;
            }
        }

        protected override void OnFed()
        {
            if (Health >= WalkingThreshold)
            {
                _wasUnableToWalkLastHour = false;
            }
        }

    }
}
