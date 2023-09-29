using System.Threading;
using System.Threading.Tasks;
using UIKit;

namespace Microsoft.Maui.Platform
{
	public static class ImageViewExtensions
	{
		public static void Clear(this UIImageView imageView)
		{
			// stop the animation if there is one
			imageView.StopAnimating();
			imageView.Image = null;
		}

		public static void UpdateAspect(this UIImageView imageView, IImage image)
		{
			imageView.ContentMode = image.Aspect.ToUIViewContentMode();
			imageView.ClipsToBounds = imageView.ContentMode == UIViewContentMode.ScaleAspectFill;
		}

		public static async Task UpdateIsAnimationPlaying(this UIImageView imageView, IImageSourcePart image)
		{
			if (imageView is MauiImageView mauiImageView)
			{
				// IsAnimationPlaying set to true in the incoming imagesource
				// indicates it's an animated image.
				if (image.IsAnimationPlaying)
				{
					if (mauiImageView.IsAnimating)
					{
						mauiImageView.StopAnimating();
					}
					
					if (!imageView.IsAnimating)
					{
						var animatedImage = await ImageAnimationHelper.CreateAnimationFromImageSource(image.Source);
						mauiImageView.AnimationImages = animatedImage.GetValuesAs<UIImage>();
						mauiImageView.AnimationDuration = animatedImage.Duration;
						mauiImageView.StartAnimating();
					}
				}
				else
				{
					if (imageView.IsAnimating)
					{
						mauiImageView.StopAnimating();
					}
				}
			}
		}

		public static void UpdateSource(this UIImageView imageView, UIImage? uIImage, IImageSourcePart image)
		{
			imageView.Image = uIImage;
			imageView.UpdateIsAnimationPlaying(image).FireAndForget();
		}

		public static Task<IImageSourceServiceResult<UIImage>?> UpdateSourceAsync(
			this UIImageView imageView,
			IImageSourcePart image,
			IImageSourceServiceProvider services,
			CancellationToken cancellationToken = default)
		{
			float scale = imageView.Window?.GetDisplayDensity() ?? 1.0f;

			imageView.Clear();
			return image.UpdateSourceAsync(imageView, services, (uiImage) =>
			{
				imageView.Image = uiImage;
			}, scale, cancellationToken);
		}
	}
}