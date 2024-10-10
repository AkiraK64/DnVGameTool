
namespace DnVCorp
{
    namespace Tools
    {
        namespace Btn
        {
            public class ExitButton : BaseButton
            {
                protected override void InteractController()
                {
                    base.InteractController();
                    if (useInteractHandle && interactHandle != null) interactHandle.Handle(interactable);
                }
            }
        }
    }
}
