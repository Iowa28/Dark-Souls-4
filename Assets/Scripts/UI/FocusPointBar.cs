namespace DS
{
    public class FocusPointBar : StatBar
    {
        public void SetMaxFocusPoints(float focusPoints)
        {
            slider.maxValue = focusPoints;
            slider.value = focusPoints;
        }

        public void SetCurrentFocusPoints(float focusPoints)
        {
            slider.value = focusPoints;
        }
    }
}