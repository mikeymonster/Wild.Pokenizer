# Pokenizer

A project for learning how to recognize Pokemon using deep learning and to deploy the model to a mobile device.

* R
* Tensorflow 
* Keras
* Xamarin Forms

## Requirements

* Visual Studio with Data Science Workload 
* R and RStudio for R project
* GPU enabled with CUDA
* Python with Tensorflow

## Project Structure

NuGet - need Xam.Plugin.Media in order to use the camera

Thoughts:

 -- .Application (or Core) - contains the types, interfaces, default predictor etc.
 -- Android model for Tensorflow
 -- UI (Xamarin Forms) - no need for additional MVVM fx here. Loads pictures from file or camera and calls search.
 -- Learning R project - can this be mixed in? And can it use the CRAN versions of R rather than the MS?

DI - use Autofac. See https://www.jamesalt.com/getting-started-with-autofac-and-xamarin-forms/

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

[Downloading Kaggle zip files in R - Stack Overflow](https://stackoverflow.com/questions/35303779/downloading-kaggle-zip-files-in-r)

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



## OLD Keras Installation Notes

> The notes below are out of date - see section at the end

https://www.tensorflow.org/install/gpu
The following NVIDIA® software must be installed on your system:

NVIDIA® GPU drivers —CUDA 9.0 requires 384.x or higher.
CUDA® Toolkit —TensorFlow supports CUDA 9.0.
CUPTI ships with the CUDA Toolkit.
cuDNN SDK (>= 7.2)

NVIDIA developer login - wildconsultingltd@gmail.com S$

Download CUDA from https://developer.nvidia.com/cuda-90-download-archive?target_os=Windows&target_arch=x86_64 
Download cuDNN from https://developer.nvidia.com/rdp/cudnn-download - Download cuDNN v7.4.2 (Dec 14, 2018), for CUDA 9.0 - Windows 10

Following https://towardsdatascience.com/python-environment-setup-for-deep-learning-on-windows-10-c373786e36d1
Installed base installer, followed by all patches in turn (not sure if this was needed)

 * custom install, 
    ** skipping driver install
    ** deseleted Visual Studio Integration, as the install failed otherwise.

To build samples, https://www.olegtarasov.me/how-to-build-cuda-toolkit-projects-in-visual-studio-2017/
 - installed VC++ 2015.3 v140 toolset under Compilers, build tools and runtimes
 - .NET 3.5 was already installed
 - extracted CUDA files and copied all files from `CUDAVisualStudioIntegration\extras\visual_studio_integration\MSBuildExtensions` to `C:\Program Files (x86)\MSBuild\Microsoft.Cpp\v4.0\v140\BuildCustomizations`
 - Modified cdeviceQuery_vs2017.vcxproj set:
    ` <CUDAPropsPath Condition="'$(CUDAPropsPath)'==''">$(VCTargetsPath14)\BuildCustomizations</CUDAPropsPath>`
 - opened VS2017 sln inC:\ProgramData\NVIDIA Corporation\CUDA Samples\v9.0\1_Utilities\deviceQuery
 - retargeted solution (right click on solution) to latest Windows 10 SDK

CdNN - download and unzip, then open an adminstrator prompt and copy:

```
copy c:\temp\cuda\bin\cudnn64_7.dll "C:\Program Files\NVIDIA GPU Computing Toolkit\CUDA\v9.0\bin"
copy c:\temp\cuda\include\cudnn.h "C:\Program Files\NVIDIA GPU Computing Toolkit\CUDA\v9.0\include"
copy c:\temp\cuda\lib\x64\cudnn.lib "C:\Program Files\NVIDIA GPU Computing Toolkit\CUDA\v9.0\lib\x64"

REM check:
dir "C:\Program Files\NVIDIA GPU Computing Toolkit\CUDA\v9.0\bin\cudnn64_7.dll"
dir "C:\Program Files\NVIDIA GPU Computing Toolkit\CUDA\v9.0\include\cudnn.h"
dir "C:\Program Files\NVIDIA GPU Computing Toolkit\CUDA\v9.0\lib\x64\cudnn.lib"

```

Made sure PATH has the following, adding where needed:

```
C:\Program Files\NVIDIA GPU Computing Toolkit\CUDA\v9.0\bin
C:\Program Files\NVIDIA GPU Computing Toolkit\CUDA\v9.0\libnvvp
C:\Program Files\NVIDIA GPU Computing Toolkit\CUDA\v9.0\include
C:\Program Files\NVIDIA GPU Computing Toolkit\CUDA\v9.0\extras\CUPTI\libx64
```


## Python and Tensorflow Installation Notes
> Updated

See https://www.pugetsystems.com/labs/hpc/How-to-Install-TensorFlow-with-GPU-Support-on-Windows-10-Without-Installing-CUDA-UPDATED-1419/

Install Anaconda

Made sure PATH has the following, adding where needed:

```
C:\Users\mike\Anaconda3\
C:\Users\mike\Anaconda3\Scripts
C:\Users\mike\Anaconda3\Library\bin
```

Update your base Anaconda packages from the Anaconda Powershell prompt:

```
conda update conda
conda update anaconda
conda update python
conda update --all
```

Create environment in Anaconda powershell:

```conda create --name tf-gpu```

(Note - environment name might be different, e.g. `tf-2-gpu`)


List environments:

```conda info --envs``` 

Activate:

```conda activate tf-gpu```

Install Tensorflow:

```conda install tensorflow-gpu```


Install Jupyter
```
conda install ipykernel jupyter
python -m ipykernel install --user --name tf-2-gpu --display-name "TensorFlow-GPU-2"
```

The above will install numpy and scipy. 
Other packages that might be needed:
```
conda install matplotlib pandas
conda install scikit-learn
conda install seaborn
conda install statsmodels
```




Removing environments

```
conda info --envs
conda remove --name <myenv> --all
```

```
conda install Pillow
```

To check python environment, open Anaconda powershell

```
conda info --envs
conda activate tf-gpu
python

import tensorflow as tf
#tf.enable_eager_execution()
print( tf.constant('Hello from TensorFlow ' + tf.__version__) )
```




## Install Tensorflow in R:

```
install.packages("keras")
library(keras)
install_keras(tensorflow = "gpu")
```


```
library(keras)
# Use specific environment already set up in Anaconda 
use_condaenv("tf-gpu")

```

The R project has been updated to set this environment

May need to install PIL/Pillow to avoid errors in running Keras models (https://stackoverflow.com/questions/48225729/importerrorcould-not-import-pil-image-working-with-keras-ternsorflow/50775336):



# Tensorflow Lite

https://www.tensorflow.org/lite
https://github.com/tensorflow/tensorflow

[Android quickstart | TensorFlow Lite](https://www.tensorflow.org/lite/guide/android)
[TensorFlow Lite example](https://www.tensorflow.org/lite/models/image_classification/android)

[Building a Custom Machine Learning Model on Android with TensorFlow Lite](https://riggaroo.co.za/building-a-custom-machine-learning-model-on-android-with-tensorflow-lite/)
[TensorFlow Lite for Android Coding on YouTube](https://youtu.be/JnhW5tQ_7Vo)

[Xamarin tf.lite input objects](https://stackoverflow.com/questions/52889099/xamarin-tf-lite-input-objects)

[reticulate: R interface to Python](https://blog.rstudio.com/2018/03/26/reticulate-r-interface-to-python/)

# TODO

Look at [Using Pre-Trained Models](https://cran.rstudio.com/web/packages/keras/vignettes/applications.html)
[Transfer Learning using Mobilenet and Keras” by Ferhat Culfaz](https://link.medium.com/DE2ufPSD1W)
[Transfer Learning with Keras in R – Florian Teschner](http://flovv.github.io/Logo_detection_transfer_learning/)

# Convert

https://www.tensorflow.org/lite/convert/python_api

```
cd \Users\mike\source\repos\Wild.Pokenizer\data

conda activate tf-gpu
python

import tensorflow as tf

converter = tf.lite.TFLiteConverter.from_keras_model_file("InceptionV3_Pokemon.h5")
tflite_model = converter.convert()
open("Pokemon_Model.tflite", "wb").write(tflite_model)


# To check or set working directory in python:
import os
os.getcwd()
os.chdir('C:\\Users\\mike\\source\\repos\\Wild.Pokenizer\\data')
```
