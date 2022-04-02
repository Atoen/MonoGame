namespace MonoGame
{
    public class PlayerShip : Entity
    {
        private static PlayerShip _instance;
        public static PlayerShip Instance => _instance ??= new PlayerShip();

        private PlayerShip()
        {
            Image = Art.Player;
            Position = GameRoot.ScreenSize / 2;
            Radius = 10;
        }

        public override void Update()
        {
            
        }
    }
}