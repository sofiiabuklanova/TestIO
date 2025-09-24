# TestIO

TestIO is a desktop console application, which takes files from the same folder as TestIO.exe. Default file names are "system_input_file.1630412935.txt" and "system_output_file.1630412935.txt". 

It checks data in them and by default creates or rewrites a resulting file "test_report.txt", which consists of lines with test result for each set of input data. 

According to requirements, TestIO logs a reason of FAIL test into console output. 

In order to try this program themself, one should download source code and build the application, create test data and run the program. To run the program with non-standard filenames or paths, use console line to pass arguments: 

`TestIO.exe input.txt output.txt test.txt`
