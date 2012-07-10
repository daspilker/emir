EmIR
====

A tool to map actions received from the [Emotiv][1] neuroheadset to IR codes 
sent with [USB-UIRT][2].

![EmIR Screenshot][6]

See the [EmIR Wiki][5] for more details.

Compiling
---------

To compile this project you can use [Visual C# 2010 Express][4].

You need several additional DLLs which may not be distributed due to licensing
restrictions. Copy the following files from the Emotiv SDK installation
directory to the project directory:

* `DotNetEmotivSDK.dll`
* `edk.dll`
* `edk_utils.dll`

Sponsors
-------

Good School

[![Good School Logo](http://www.good-school.de/themes/good_school/logo.png)](http://www.good-school.de/)

[http://www.good-school.de/](http://www.good-school.de/)

License
-------

	Copyright 2012 Daniel A. Spilker
	
	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at
	
	    http://www.apache.org/licenses/LICENSE-2.0
	
	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.

Some icons are provided by the [Silk icon set][3].

UsbUirtManagedWrapper.dll provided by [USB-UIRT][2].

[1]: http://www.emotiv.com/
[2]: http://www.usbuirt.com/
[3]: http://www.famfamfam.com/lab/icons/silk/
[4]: http://www.microsoft.com/express/Downloads/#2010-Visual-CS
[5]: http://github.com/daspilker/emir/wiki
[6]: http://farm2.static.flickr.com/1361/5114861583_4ca36a1c67.jpg
