namespace IDAL
{
    namespace DO
    {
        public enum CHOICE
        {
			ADD,UPDATE, DISPLAY, VIEW_LIST, EXIT
        }
        public enum WeightCategories
        {
            Light=1, Medium, Heavy
        }

        public enum Priorities {Regular=1, Express, Urgent }
        public enum DroneStatuses {Available=1, Delivery, Charging }
        public enum CustomerName {Avi, Benny, Gadi, Danny, Freddy, Alex, Nati, Oren, channan, Yair, Tal, Noam}
        public enum NameDrone {mavic, tello, syma, flybird, lion, cobra, cheetah, beck, worm, bean}   
    }
}