AutoGrid
========

This is a replacement for the Grid in WPF/Silverlight and presumably things like Win8. Having written it to scratch an itch it looks like there are others out there (even with the same name), so you probably want to use those ones. I got the inspiration from outside the Windows ecosystem (Android from memory).

Basically it will just fill up row by row without you having to define Grid.Row and Grid.Column on all your children, which is particularly nice when you decide you want to fiddle the layout of your page since you don't need to bloody renumber everything <sup><sub>grumble grumble grumble</sub></sup>.

Show Me Teh Codez!!11!
======================
Two auto width columns and as many auto height rows as you need. Add RowHeight="*" or ColumnWidth="*" to change the default. As a gotcha, not setting NumColumns gives you a single "*" column but setting NumColumns="1" gives you a single auto column. The former is to fit in with the standard grid a bit better. In generally the defaults are auto though.
```
        <a:AutoGrid NumColumns="2">
            <TextBlock Text="First Row"/>
            <TextBlock Text="First Row"/>
            
            <TextBlock Text="Second Row"/>
            <TextBlock Text="Second Row"/>

            <TextBlock Text="Third Row"/>
            <TextBlock Text="Third Row"/>
        </a:AutoGrid>
```
More commonly I use explicit column definitions and auto rows, but you can define the widths in a single string like this:
```
        <a:AutoGrid Columns="auto,*,50">
            <TextBlock Text="First Row"/>
            <TextBlock Text="First Row"/>            
            <TextBlock Text="First Row"/>

            <TextBlock Text="Second Row"/>
            <TextBlock Text="Second Row"/>
            <TextBlock Text="Second Row"/>
        </a:AutoGrid>
```
You can define the rows in the same way if you don't want them all auto or all *.

[![Build status](https://ci.appveyor.com/api/projects/status/68klhicrolclel24/branch/master?svg=true)](https://ci.appveyor.com/project/markryd/autogrid/branch/master)



