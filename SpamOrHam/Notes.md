Notes for Naive Bayesian Classification
=======================================

1. Use case: Use vocabulary frequency to classify document types.

2. Always spend time exploring the data at hand! This will correctly help model the domain.

3. A boolean can easily be replaced by Discriminated Unions in F#. In fact they provide a lot more domain specific information just based on their type. 

4. Using File.ReadAllLines isn't space efficient.. Probably best to use StreamReader in that case since it loads the data as a stream, one line at a time. 

5. Tranformation of the text into some features is the way to achieve the classification. 

6. Use decision trees to model the proportions vs. words for each path eg. Spam or Ham in this case.  

7. Bayes Theorem is cool.. 

8. Laplace Smoothing: Fudge factor to prevent 100% probabilities in case words just don't exist in the other class of our data set. 

9. Tokenization is the way to break up a single string into multiple words.

10. Naive => Order doesn't matter.. 

11. For multiple words, simply multiply the words occuring together i.e. ( P( "A" | E ) x P ( "B" | E ) x P ( "C" | E ))

12. Multiplying too many numbers 0 < x < 1 will lead to many rounding errors. This can be changed by using logs since, log ( x * y ) = log ( x ) + log ( y )