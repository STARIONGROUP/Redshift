Redshift is a framework encompassing various helper libraries used to implement server side applications. The following libraries are made available under the [GNU LGPL](https://www.gnu.org/licenses/lgpl.nl.html):

  - Redshift.Orm
  - Redshift.Api
  
## Build Status

AppVeyor is used to build and test Redshift

Branch | Status | Coverage
------------ | ------------- | -------------
master | [![Build status](https://ci.appveyor.com/api/projects/status/v2tfo0nefpncq9bv/branch/master?svg=true)](https://ci.appveyor.com/project/alexatrhea/redshift/branch/master) | [![codecov](https://codecov.io/gh/RHEAGROUP/Redshift/branch/master/graph/badge.svg)](https://codecov.io/gh/RHEAGROUP/Redshift)
development | [![Build status](https://ci.appveyor.com/api/projects/status/v2tfo0nefpncq9bv/branch/development?svg=true)](https://ci.appveyor.com/project/alexatrhea/redshift/branch/development) | [![codecov](https://codecov.io/gh/RHEAGROUP/Redshift/branch/development/graph/badge.svg)](https://codecov.io/gh/RHEAGROUP/Redshift)

[![Build history](https://buildstats.info/appveyor/chart/alexatrhea/redshift)](https://ci.appveyor.com/project/alexatrhea/redshift/history)
 
## Redshift.Orm

An object relational mapping library that enables you to connect your data model to a persistance back-end. As of now only PostgreSQL is supported, but it is envisioned for more database providers to become available.

### Installation

The librbary is available on nuget.org: https://www.nuget.org/packages/Redshift.Orm

```
Install-Package Redshift.Orm
```

## Redshift.Api
An API helper library designed to standardize interfaces and core implementations of an API based on **NancyFX** 2 and **Redshift.Orm**.

### Installation

The librbary is available on nuget.org: https://www.nuget.org/packages/Redshift.Api

```
Install-Package Redshift.Api
```

NOTE: Redshift.Api is automatically dependent on Redshift.Orm. There is no need to install the **Orm** library if the **Api** library is installed.


# License

The libraries contained in the Redshift Framework are provided to the community under the GNU Lesser General Public License v3. See the LICENSE and COPYING.LESSER files for the license text.
## Copyright Statement
Copyright (c) 2018 RHEA System S.A.

Authors: Alex Vorobiev, Naron Phou

This file is part of Redshift.Orm

Redshift.Orm is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Redshift.Orm is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Redshift.Orm.  If not, see <http://www.gnu.org/licenses/>.

# Contributions

Contributions to the code-base are welcome. However, before we can accept your contributions we ask any contributor to sign the Contributor License Agreement (CLA) and send this digitaly signed to s.gerene@rheagroup.com. You can find the CLA's in the root of the solution.
