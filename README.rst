Scrolliris Desktop for Windows
==============================

Code Name: ``Wetzikon``

.. raw:: html

   <img src="/img/scrolliris-desktop-windows-20180628.png?raw=true" alt="Scrolliris Desktop Windows v0.0.1" width="510px">


Dependencies
------------

Building
~~~~~~~~

* .NET Framework v4.7.2

* Windows 10 SDK

  * Windows 10 App Development (UWP)

* Visual Studio Build Tools 2017

  * NuGet.exe
  * MSBuild.exe

    * MakeAppx.exe

Testing
~~~~~~~

* VSTest (For run, it needs Visual Studio 2017 Installation)


Build
-----

.. code:: powershell

   > NuGet.exe restore "Wetzikon/Wetzikon.csproj"
   > PowerShell.exe -ExecutionPolicy Bypass -File .\Build.ps1 `
       -Configuration Debug -Platform x64 -Run -Clean

Logging
~~~~~~~

In **DEBUG**, a log file named ``debug.log`` will be created.

.. code:: bash

   # on WSL
   $ grc -c conf.debug tail -f /mnt/c/Users/<USER>/AppData/Local/Packages/ \
     <PACKAGE>/LocalCache/debug.log

   $ ./taillog


If you want, check following ``conf.debug`` for grc.

.. code:: text

   regexp=^.*\s(TRACE)\s.*$
   colours=unchanged,"\033[38;5;117m"
   count=once
   -
   regexp=^.*\s(DEBUG)\s.*$
   colours=unchanged,"\033[38;5;183m"
   count=once
   -
   regexp=^.*\s(INFO)\s.*$
   colours=unchanged,"\033[38;5;228m"
   count=once
   -
   regexp=^.*\s(WARN)\s.*$
   colours=unchanged,"\033[38;5;203m"
   count=once
   -
   regexp=^.*\s(ERROR)\s.*$
   colours=unchanged,"\033[38;5;199m"
   count=once
   -
   regexp=^.*\s(FATAL)\s.*$
   colours=unchanged,"\033[38;5;1m"
   count=once



Test
----

.. code:: powershell

   > NuGet.exe restore "Wetzikon.Tests/Wetzikon.Tests.csproj"
   > PowerShell.exe -ExecutionPolicy Bypass -File .\RunTest.ps1 `
       -Platform x64 -Clean



License
-------

See LICENSE (``GPL-3.0``).

::

   Scrolliris Desktop for Windows
   Copyright (c) 2018 Lupine Software LLC


| This program is free software: you can redistribute it and/or modify
| it under the terms of the GNU General Public License as published by
| the Free Software Foundation, either version 3 of the License, or
| (at your option) any later version.
|
| This program is distributed in the hope that it will be useful,
| but WITHOUT ANY WARRANTY; without even the implied warranty of
| MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
| GNU General Public License for more details.
|
| You should have received a copy of the GNU General Public License
| along with this program.  If not, see <http://www.gnu.org/licenses/>.
