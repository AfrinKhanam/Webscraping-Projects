<!--- Make sure to update this training data file with more training examples from https://forum.rasa.com/t/grab-the-nlu-training-dataset-and-starter-packs/903 --> 

## intent:bye <!--- The label of the intent --> 
- Bye 			<!--- Training examples for intent 'bye'--> 
- Goodbye
- See you later
- Bye bot
- Goodbye friend
- bye
- bye for now
- catch you later
- gotta go
- See you
- goodnight
- have a nice day
- i'm off
- see you later alligator
- we'll speak soon

## intent:greet
- Hi
- Hey
- Hi bot
- Hey bot
- Hello
- Good morning
- hi again
- hi folks
- hi Mister
- hi pal!
- hi there
- greetings
- hello everybody
- hello is anybody there
- hello robot



## intent:affirm
- yes
- yes sure
- absolutely
- for sure
- yes yes yes
- definitely


## intent:name
- My name is [Alice](name)  <!--- Square brackets contain the value of entity while the text inside the parentheses is a a label of the entity --> 
- I am [Josh](name)
- I'm [Lucy](name)
- People call me [Greg](name)
- It's [David](name)
- Usually people call me [Amy](name)
- My name is [John](name)
- You can call me [Sam](name)
- Please call me [Linda](name)
- Name name is [Tom](name)
- I am [Richard](name)
- I'm [Tracy](name)
- Call me [Sally](name)
- I am [Philipp](name)
- I am [Charlie](name)


## intent:loan_types
- types of loan
- what are the types of loan
- name the types of loan
- loan types
- can you please tell the types of loan your bank is having
- may i know the types of loan 
- can you display the types of loan 

## intent:restaurant_search
- I am looking for [Chinese](food_type) food
- I am looking for [indian](food_type) food
- I am looking for [italian](food_type) food
- I am lloking for [american](food_type) food

## intent:loans
- i want to apply for loan
- i want to apply for [agriculture](agriculture) loan
- i want to apply for [agri](agriculture) loan
- [agriculture](agriculture) loan
- [agri](agriculture) loan
- [agriculture](agriculture)
- procedure for applying [agriculture](agriculture) loan
- i want to apply for [personal](personal) loan
- i want to apply for [individual](individual) loan
- i want to apply for [personal/individual](personal/individual) loan
- i want to apply for [sme](sme) loan
- i want to apply for [nri](nri) loan
- i want to apply for [59 minutes](59 minutes) loan
- i want to apply for [59](59 minutes) loan
- [59 minutes](59 minutes)
- loans
- loan




