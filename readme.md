**Description:**

This console app produces aggregates of counts and sums over the fields 'Network', 'Product' and 'Month' in a csv file.

**Usage:**

Run the exe in command prompt specifying the file location.
Administrator mode for command prompt may be required.
If the file exists in the same folder as the exe the usage is as follows:
```
LoadAggregator.exe Loans.csv
```
**Assumptions:**

* The file format is consistent.
* The first line contains the column names.
* Column values are consistent and dont contain blanks.
* The date format is consistent.
* Example of data row: '27729554427,'Network 1','12-Mar-2016','Loan Product 1',1000.00'
* Path of executable is writable to produce the output file.

**Code:**

This program is written in .net and C#.
No external libraries were used and minimal dependencies are referenced.

**Notes:**

In the example Loans.csv, the tuple of 'Network', 'Product' and 'Month' is unique within the data.
This means no meaningful aggregate is being performed, a data set containing non-unique tuples of
'Network', 'Product' and 'Month' is required for this to be useful.
