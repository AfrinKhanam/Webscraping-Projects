from MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe
import json
import uuid


test_query = "per day premium to be paid for a  family towards health care"


test_query = "premium subsidy for an individual having health care family"
test_query = "benefits per annum"

test_query = "what amount can a person claim in case of accident in health care"

test_query = "per annum premium to be paid by individuaaal  for health care"
test_query = "requirement for mca payment"
test_query = "requirement for money gram"


#test_query = "What is the rate of interest for Financing agriculturists on loans involving the purchase of tractors? "
test_query = "interest rate"

test_query = 'what come under agriculture godown or cold storage loans?'
test_query = "how to remittence money neft"
test_query = 'what is the agricultural jewel loan scheme?'
test_query = 'please provide some details regarding 59 mins loan'

test_query = 'Please provide some details regarding IB home loans'
test_query = 'What is the eligibility to apply for an IB home loan?'
test_query = 'home loan'


test_query = 'What can you tell me about the IB home loan combo?'
test_query = 'What can you tell me about IB home loans?'
test_query = 'What can you tell me about  Home Improve Loan?'
test_query = 'What can you tell me about home loan plus'

test_query = 'What can you tell me about  IB vehicle loans?'
test_query = 'What are the Agriculture loan products that are offered by Indian Bank? '
test_query = 'What are the type of agriculture loan products offered by the bank?'
test_query = 'What is the eligibility to get an agriculture loan?'
test_query = 'How to apply for an agriculture loan?'
test_query = 'Criteria to apply for Agriculture loan'
test_query = 'What comes under agriculture godowns or cold storage loans?'
test_query = 'What is the purpose of a cold storage loan?'
test_query = "What is the purpose of  the loans for maintenances of tractors under tie up with sugar mills?"
test_query = "What are the Loans for maintenance of tractors under tie up with sugar mills?"

test_query = "What is the  tenure of repayment for an Agricultural produce Marketing Loan?"
test_query = "What is the security deposit required for an Agricultural produce Marketing Loan?"
test_query = "What is the maximum term of repayment for loans associated with the maintenance of tractors under tie up with sugar mills?"
test_query = "What is the rate of interest for Financing agriculturists on loans involving the purchase of tractors? "
test_query = "what is"
test_query = "How can I apply for doorstep banking?"


test_query = "e payment of indirect tax"

test_query = "How much profit does IB make?"

test_query = "can a customer pay tax on internet"
test_query = "who is the managing director and ceo of indian bank "
test_query = "What are the objectives that fall under godown or cold storage loans?"
test_query = "agricultural"
test_query = "What is the agricultural Jewel Loan Scheme?"
test_query = "What awards have Inidan bank won?"
test_query = "What is the Vision of Indian Bank"
test_query = "target customers in POS?"
test_query = "What awards have Inidan bank won?"
test_query = "What is the Quarterly Financial Result of the bank?"
test_query = "Financial Result"
test_query = "Who is the RBI Nominee?"
test_query = "What is the security required for Financing agriculturists"
test_query = "What is doorstep banking?"
test_query = "What services does doorstep banking include?"
#test_query = "doorstep"
test_query = "What are the services door step banking?"
test_query = "who is the managing director and ceo of indian bank "
test_query = "What is doorstep banking?"
test_query = "What services does doorstep banking include?"

test_query = "who is the coe of the bunk"
test_query = "i want to buy a home"
test_query = "family premium for health care"
test_query = "what is door step banking"
test_query = "managing director"
test_query = "family premium subsidy universal health care"
test_query = "Financing Agriculturists for Purchase of/Tractors \""
test_query = "rd"
test_query = "who is rbi nominee"
test_query = "md"
test_query = "who is share holder director"
test_query = "single premium per annum to be paid against health care"
test_query = "i waant to buy home"
test_query = "open sb account"
test_query = "i am a doctor can i get loan"
test_query = "senior citizen fd"
test_query = "age for jeevan kalyan"
test_query = "premium for jeevan kalyan"
test_query = "scheme for education loan for poor student"
test_query = "premium for an individual in health care"
test_query = "interest rate"
test_query = "premium for jeevan kalyan"
#est_query = "rbi kehta hai"



test_query = " internet payment on direct tax"
test_query = "how to apply home loan plus"
test_query = "how to apply home loan combo"
test_query = "home loan improve"
test_query = "Home loan"
test_query = "interest on home lona combo"
test_query = "car loan"
test_query = "truck loan"
test_query = "interest for Home loan combo"
test_query = "pre used tractor"




test_query = "bal vidya scheme"
test_query = "purpose of bal vidya scheme"

test_query = "jeevan vidya scheme"
test_query = "purpose of jeevan vidya scheme"



test_query = "document required for education loan"
test_query = "document required for education loan"
test_query = "education loan amount of loan for list a"
test_query = "education loan for student"

test_query = "per day rate for a single person health"
test_query = "education loan security"

test_query = "age for jeevan kalyan"

test_query = "education loan margin"
test_query = "home loan"
test_query = "i need information on vykal loan"

test_query = "education loan prime  interest  for list B"
test_query = "education loan prime  interest  for list B"
test_query = "education loan prime  amount of loan"

test_query = "security deposit to be paid for education loan"
test_query = "security deposit to be paid for education loan"

test_query = "argriculture jewel loan"
test_query = "produce marketing loan"

test_query="what is the eligibility to apply for a godown or cold storage"
test_query = "what is the eligibility to apply for a godown or cold storage"
test_query = "Indian bank profile"
test_query = "About Indian bank"
test_query = "Name of the subsidiary companies of Indian bank" #match via subtitle and prioritize the documents accordingly
test_query = "With whom all indian bank tie up for sale of mutual fund products" #working fine
test_query = "Do Indian bank have international branches?" #working as expected
test_query = "motor cycle bike"
test_query = "study loan"
test_query = "four-wheeler loan"
test_query = "mobike loan"
test_query = "What is the eligibility to apply for a godown or cold storage loan?"
test_query = "What is the criteria to apply for a godown or cold storage loan?"
test_query = "2975"


test_query = "What is the loan eligibility to apply for a godown or cold storage?"
test_query = "security for 2 wheeler loan"
test_query = "What is the interest rate on godown or cold storage loans?"
test_query = "What is the criteria to apply for a godown or cold storage loan?"
test_query = "What is the margin on a cold storage loan?"
test_query = "What is the security required for a godown or cold storage loan?"
test_query = "What is the maximum term for repayment for godown or cold storage loans?"
test_query = "What is the maximum amount of loan permissible under godown or cold storage loans?"


test_query = "What is the purpose of the loans for maintenances of tractors under tie up with sugar mills?"
test_query = "What is the eligibility of Loans for maintenances of tractors under tie up with sugar mills?"
test_query = "What is the margin on the Loans for maintenances of tractors under tie up with sugar mills?"
test_query = "What are the Loans for maintenance of tractors under tie up with sugar mills?"
test_query = "how can i avail for agriculture produce loan?"
test_query = "What is the maximum tenure of repayment as per the DRI Scheme-Revised Norms?"
test_query = "What is the security deposit required for the DRI Scheme-Revised Norms?"
test_query = "What are the revised targets for a DRI loan?"
test_query = "What is the security required for Loans associated with the maintenances of tractors under tie up with sugar mills?"
test_query = "What is the criteria to apply for a godown or cold storage loan?"
test_query = "criteria for gowdowns or cold storage loan"
# test_query = "what is the maximum term on fixed deposit"-->200-->No Maximum
# test_query = "what is the lowest amount for fixed deposit"-->200--->Minimum Deposit- Rs.100/-
# test_query = "when interest is paid on fixed deposit"-->200-->\nSalient Features\n
# test_query = "can i get monthly interest on fixed deposit"--20--->\nSalient Features\n
test_query = "can I get payment of fixed before maturity"

test_query = "when interest is paid on fixed deposit"
test_query = "what is the maximum term on fixed deposit"
test_query = "criteria for gowdowns or cold storage loan"
test_query = "What are the salient features of Fixed Deposit? "

	
# details	,info,specifics,feature

# ..................JYOTHI SIR.................
test_query = "Eligibility for car loan"
# test_query = "rickshaw loan" check
test_query = "scooter loan"
# test_query = "cycle rickshaw loan" not working
# test_query = "What is DRI Scheme" rasa
# test_query = "Loans under DRI Scheme" rasa
# test_query = "DRI Scheme Revised Norms" rasa
# test_query = "Maximum Loan amount under DRI Scheme" rasa
test_query = "Interest uander DRI Scheme"
test_query = "what is the minimum amount on fixed deposit"
test_query = "criteria to apply for agriculture godowns or cold storage loan"
test_query = "Eligibility for car loan"
test_query = "Interest Rate on Jewel Loan"
test_query = "security for 2 wheeler loan"
test_query = "eligibility on agriculture godowns or cold storage loan"
test_query = "What is the eligibility to apply for a godown or cold storage loan?"
test_query = "amount of loan on agriculture godowns"

test_query = "min amt of fixed deposit"
test_query = "Who is the Managing Director of Indian Bank?"
test_query = "Who is the Managing Director of Indian Bank?"
test_query = "What are service charges on Forex Rate Cards?"

test_query = "What are the service charges on Forex transactions?"
test_query = "What is the rent on lockers?"
test_query = "What are the Foreclosure charges applicable on premature withdrawal of Term Deposits?"

































test_query_string = {
    "UUID" : "absdj-1237812-asdjdas",
    "QUERY_STRING" : test_query,
    "CONTEXT" : ""

    }
rabbitmq_producer_query = RabbitmqProducerPipe(publish_exchange="queryExchange", 
        routing_key="query",
        host="localhost")
rabbitmq_producer_query.publish(json.dumps(test_query_string).encode())
