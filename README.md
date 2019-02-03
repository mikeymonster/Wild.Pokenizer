# Pokenizer

A project for learning how to recognize Pokemon using deep learning and to deploy the model to a mobile device.

* R
* Tensorflow 
* Keras
* Xamarin Forms

## Requirements

Visual Studio with Data Science Workload for R project
GPU enabled with CUDA
Keras and Tensorflow

## Project Structure

NuGet - need Xam.Plugin.Media in order to use the camera

Thoughts:

 -- .Application (or Core) - contains the types, interfaces, default predictor etc.
 -- ? Android model? for Tensorflow
 -- UI (Xamarin Forms) - no need for additional MVVM fx here. Loads pictures from file or camera and calls search.
 -- .Learning r PROJECT - can this be mixed in? And can it use the CRAN versions of R rather than the MS?

DI - use Autofac

Unit tests - xunit, with NSubstitute

Camera - use [MediaPlugin](https://github.com/jamesmontemagno/MediaPlugin#important-permission-information) - note the complicated permission set up:

## CSS Stylesheets
https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/styles/css/
https://xamarinhelp.com/css-xamarin-forms/
https://visualstudiomagazine.com/articles/2018/04/01/styling-xamarin-forms.aspx
https://forums.xamarin.com/discussion/138886/can-i-code-for-a-xamarin-forms-css-style-sheet-base-on-platform-can-i-target-a-style-sheet


## Links

MSDN magazine [Cognitive sample](https://msdn.microsoft.com/magazine/mt742868) with camera

[MVVM](https://docs.microsoft.com/en-gb/xamarin/xamarin-forms/xaml/xaml-basics/data-bindings-to-mvvm)

* [DI](http://www.codenutz.com/autofac-ninject-tinyioc-unity-with-xamarin-forms-labs-xlabs/)
* https://www.rizamarhaban.com/2017/10/10/using-autofac-in-xamarin-forms-mobile-apps/
* https://www.jamesalt.com/getting-started-with-autofac-and-xamarin-forms/

DI with [Unity](https://blogs.msdn.microsoft.com/mvpawardprogram/2017/10/03/dependency-injection-xamarin/)

[CSS per platform](https://forums.xamarin.com/discussion/138886/can-i-code-for-a-xamarin-forms-css-style-sheet-base-on-platform-can-i-target-a-style-sheet)

Something about Tensorflow with different sized images: https://stackoverflow.com/questions/41907598/how-to-train-images-when-they-have-different-size 

Using TensorFlow and Azure to Add Image Classification to Your Android Apps from [Xamarin Blog](https://blog.xamarin.com/android-apps-tensorflow/)

Identifying my daughters toys using AI - [Part 4](https://www.jimbobbennett.io/identifying-my-daughters-toys-using-ai-part-4-offline-android/), using the models offline on Android
 (uses https://github.com/lobrien/TensorFlow.Xamarin.Android)

For charts look at nuget package Microcharts and Microcharts.Forms


## Deploy to Android

Connect your phone to PC using USB Cable. If succesfully connected, you will find your device in emulators list. Select your device and Run. Application will launch on your device.
Other way is to manually create a .apk and and copy that file on to device and install it. This method is lot time consuming and not suitable for development purpose. Details at : http://developer.xamarin.com/guides/android/deployment,_testing,_and_metrics/publishing_an_application/part_1_-_preparing_an_application_for_release/

To set up developer mode on phone:
 Settings > About phone, and tap the Build number item seven times to reveal the Developer Options tab

Then the Developer Options should be available in Settings (possibly under System)

Can turn on USB Debugging options from there.


## Tensorflow Deployment

Official docs on [Deploying TensorFlow Models](https://www.tensorflow.org/lite/tfmobile/prepare_models)

[tfdeplpoy](https://tensorflow.rstudio.com/tools/tfdeploy/articles/introduction.html)

[export Keras model to .pb](https://stackoverflow.com/questions/49474467/export-keras-model-to-pb-file-and-optimize-for-inference-gives-random-guess-on)

More up-to-date [info](https://link.medium.com/vx2c2Hh1GT) - TensorFlow Lite Now Faster with Mobile GPUs (Developer Preview)” by TensorFlow 

https://github.com/emgucv/emgutf/commit/6df07424da4b9ce5b64ecad95b40a11408f1416f