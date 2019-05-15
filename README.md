# UIPath-JS

Ability to run javascript (nodejs) in UIPath designer (still under development,  more functionality will be added in upcoming release..!)

[Download nuget package here](https://drive.google.com/file/d/1ibM7HxcQWyRCL4AGMM0fDA4EbSkBTRbS/view?usp=sharing)


![](https://raw.githubusercontent.com/vinojash/UIPath-JS/master/Screenshots/palindrome.gif)

[Thanks to EdgeJS](https://www.nuget.org/packages/Edge.js/)

[Click here to Edge git repository](https://github.com/tjanczuk/edge)

# How to use
    Download and install the above nuget package to your designer then, Create a Blank Process in UIPath Designer,then add Input Dialog, Execute Javascript and Message Box activity and connect those three sequentially. 

![](https://raw.githubusercontent.com/vinojash/UIPath-JS/master/Screenshots/1%20workflow.PNG)

    Create two string variables called "data" and "output"

![](https://raw.githubusercontent.com/vinojash/UIPath-JS/master/Screenshots/2%20Variables.PNG)

    Get any string value from user and store it in string variable "data" using Input dialog activity.

![](https://raw.githubusercontent.com/vinojash/UIPath-JS/master/Screenshots/3%20InputDialog.PNG)

    Pass the "data" variable value to "Data" input of execuite java script activity,(Execute javascript activity internally uses one variable name called data, which is outout of the same activity)

![](https://raw.githubusercontent.com/vinojash/UIPath-JS/master/Screenshots/4%20runjs.PNG)

Pass the static string format of javascript function to "JSFunction" input of execuite java script activity

![](https://raw.githubusercontent.com/vinojash/UIPath-JS/master/Screenshots/4.1%20js%20expression%20editor.PNG)

# Javascript

```javascript
function palindrome(str) {
    var re = /[\W_]/g;
    var lowRegStr = str.toLowerCase().replace(re, '');
    var reverseStr = lowRegStr.split('').reverse().join('');
    if (reverseStr === lowRegStr) {
        return 'Yes, ' + str + ' is a palindrome..!';
    } else {
        return 'No, ' + str + ' is not a palindrome..!';
    }
}
data=palindrome(data); //data will be both input and output
```
    Map the "output" variable as Result of execuite java script activity and Map the "output" variable to Message box to show the result to user..!

![](https://raw.githubusercontent.com/vinojash/UIPath-JS/master/Screenshots/5%20message%20box.PNG)

    Run the bot & try to give different kind of inputs like "MADAM", "INDIA", "APPLE", etc 
