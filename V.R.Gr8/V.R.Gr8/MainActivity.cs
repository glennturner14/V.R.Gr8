using Android.App;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using System.Collections.Generic;
using Android.Content;
using Java.IO;
using Android.Content.PM;
using Android.Net;
using Android.Provider;
using V.R.Gr8.Classes;
using static Android.Print.PrintAttributes;
using System.Net.Http;

namespace V.R.Gr8
{
    [Activity(Label = "V.R.Gr8", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        ImageView _imageView;
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            if (IsThereAnAppToTakePictures()) {
                CreateDirectoryForPictures();

                Button button = FindViewById<Button>(Resource.Id.myButton);
                _imageView = FindViewById<ImageView>(Resource.Id.imageView1);
                button.Click += TakeAPicture;
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data) {
            base.OnActivityResult(requestCode, resultCode, data);

            // Make it available in the gallery

            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Uri contentUri = Uri.FromFile(App._file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);

            // Display in ImageView. We will resize the bitmap to fit the display.
            // Loading the full sized image will consume to much memory
            // and cause the application to crash.

            int height = Resources.DisplayMetrics.HeightPixels;
            int width = _imageView.Height;
            App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
            if (App.bitmap != null) {
                _imageView.SetImageBitmap(App.bitmap);
                byte[] bitmapData;
                using (var stream = new System.IO.MemoryStream()) {
                    App.bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                    bitmapData = stream.ToArray();
                }
                //var myTask = ImageUpload.UploadUserPictureApiCommand(bitmapData);
                ImageUpload.PostItem(bitmapData);

                App.bitmap = null;
            }


            //UploadUserPictureApiCommand()
            // Dispose of the Java side bitmap.
            System.GC.Collect();
        }

        
        private void CreateDirectoryForPictures() {
            App._dir = new File(
                Environment.GetExternalStoragePublicDirectory(
                    Environment.DirectoryPictures), "CameraAppDemo");
            if (!App._dir.Exists()) {
                App._dir.Mkdirs();
            }
        }

        private bool IsThereAnAppToTakePictures() {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        private void TakeAPicture(object sender, System.EventArgs eventArgs) {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            App._file = new File(App._dir, System.String.Format("myPhoto_{0}.jpg", System.Guid.NewGuid()));
            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App._file));
            StartActivityForResult(intent, 100);
        }
    }

    public static class App {
        public static File _file;
        public static File _dir;
        public static Bitmap bitmap;


    }

    public static class BitmapHelpers {
        public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height) {
            // First we get the the dimensions of the file on disk
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(fileName, options);

            // Next we calculate the ratio that we need to resize the image by
            // in order to fit the requested dimensions.
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            if (outHeight > height || outWidth > width) {
                inSampleSize = outWidth > outHeight
                                   ? outHeight / height
                                   : outWidth / width;
            }

            // Now we will load the image and have BitmapFactory resize it for us.
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

            return resizedBitmap;
        }
    }


}

