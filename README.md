# UIMatrixML

UIMatrix ML is a dot-net framework which enables developers and QA to easily build automated tests which utilize Selenium and Machine Learning to build a matrix representation of HTML elements.

With UIMatrix ML, verification of front-end elements can easily be achieved.

## How it works

Essentially, the framework calculates matrices given x features of HTML elements. For example, the default matrix build includes POS_X, POS_Y, WIDTH, HEIGHT. Simply put, a mathematical representation of your HTML elements' features is built.

A matrix would be built representing these features for a button:

    [0, 0, 100, 25] 

UIMatrixML will build an inference model with given data to determine the probability that your site is visually stable. Running your site in different scenarios allows you to find areas where visual mishaps might happen.
