# TestIO

TestIO is a desktop console application, which takes files from the same folder as TestIO.exe. Default file names are "system_input_file.1630412935.txt" and "system_output_file.1630412935.txt". 

It checks data in them and by default creates or rewrites a resulting file "test_report.txt", which consists of lines with test result for each set of input data. 

According to requirements, TestIO logs a reason of FAIL test into console output. 

To run this program, download source code and build the application, create test data and run the application. To run the program with non-standard filenames or paths, use console line to pass arguments: 

`TestIO.exe path_to_input_file.txt path_to_output_file.txt test_report_file.txt`
