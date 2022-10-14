using PdfSignature.Controls;
using PdfSignature.UWP.Implementation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Input;
using Windows.UI.Xaml.Input;

[assembly: ResolutionGroupName("XamarinDocs")]
[assembly: ExportEffect(typeof(PdfSignature.UWP.Implementation.TouchEffect), "TouchEffect")]
namespace PdfSignature.UWP.Implementation
{
    public class TouchEffect : PlatformEffect
    {
        FrameworkElement frameworkElement;
        Controls.TouchEffect effect;
        Action<Element, TouchActionEventArgs> onTouchAction;


        
        void OnPointerPressed(object sender, PointerRoutedEventArgs args)
        {
            CommonHandler(sender, TouchActionType.Pressed, args);

            // Check setting of Capture property
            if (effect.Capture)
            {
                (sender as FrameworkElement).CapturePointer(args.Pointer);
            }
        }
         void CommonHandler(object sender, TouchActionType touchActionType, PointerRoutedEventArgs args)
        {
            PointerPoint pointerPoint = args.GetCurrentPoint(sender as UIElement);
            Windows.Foundation.Point windowsPoint = pointerPoint.Position;

            onTouchAction(Element, new TouchActionEventArgs(args.Pointer.PointerId,
                                                            touchActionType,
                                                            new Point(windowsPoint.X, windowsPoint.Y),
                                                            args.Pointer.IsInContact));
        }

        void OnPointerEntered(object sender, PointerRoutedEventArgs args)
        {
            CommonHandler(sender, TouchActionType.Entered, args);
        }

        void OnPointerMoved(object sender, PointerRoutedEventArgs args)
        {
            CommonHandler(sender, TouchActionType.Moved, args);
        }

        void OnPointerReleased(object sender, PointerRoutedEventArgs args)
        {
            CommonHandler(sender, TouchActionType.Released, args);
        }
        void OnPointerExited(object sender, PointerRoutedEventArgs args)
        {
            CommonHandler(sender, TouchActionType.Exited, args);
        }
        void OnPointerCancelled(object sender, PointerRoutedEventArgs args)
        {
            CommonHandler(sender, TouchActionType.Cancelled, args);
        }

        protected override void OnAttached()
        {
            // Get the Windows FrameworkElement corresponding to the Element that the effect is attached to
            frameworkElement = Control == null ? Container : Control;

            // Get access to the TouchEffect class in the .NET Standard library
            effect = (Controls.TouchEffect)Element.Effects.
                        FirstOrDefault(e => e is Controls.TouchEffect);

            if (effect != null && frameworkElement != null)
            {
                // Save the method to call on touch events
                onTouchAction = effect.OnTouchAction;

                // Set event handlers on FrameworkElement
                frameworkElement.PointerEntered += OnPointerEntered;
                frameworkElement.PointerPressed += OnPointerPressed;
                frameworkElement.PointerMoved += OnPointerMoved;
                frameworkElement.PointerReleased += OnPointerReleased;
                frameworkElement.PointerExited += OnPointerExited;
                frameworkElement.PointerCanceled += OnPointerCancelled;
            }
        }

        protected override void OnDetached()
        {
            return;
        }
    }
}
