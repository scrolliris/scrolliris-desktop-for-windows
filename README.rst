Scrolliris Desktop for Windows
==============================

Code Name: ``Wetzikon``


Dependencies
------------

Building
~~~~~~~~

* .NET Framework v4.7.2
* Windows 10 SDK
  * Windows 10 App Development (UWP)
* Visual Studio Build Tools 2017
  * NuGet
  * MSBuild

Testing
~~~~~~~

* MStest
* TestRunner


Build
-----

.. code:: powershell

   > PowerShell.exe -ExecutionPolicy Bypass -File .\BuildAndRun.ps1 "x64"



Test
----

.. code:: powershell

   # `packages.config` is used only for the setup of `TestRunner`.
   > .\NuGet.exe install "Wetzikon.Tests/packages.config"

   > PowerShell.exe -ExecutionPolicy Bypass -File .\RunTest.ps1 "x64"



License
-------

See LICENSE (``GPL-3.0``).

.. code:: text

   Scrolliris Desktop for Windows
   Copyright (c) 2018 Lupine Software LLC

   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program.  If not, see <http://www.gnu.org/licenses/>.
