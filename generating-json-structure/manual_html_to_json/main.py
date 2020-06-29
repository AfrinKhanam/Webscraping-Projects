from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe
import json
import sys
import array as arr

ib_surabhi_path = "./ManuallyScrapedData/IbSurabhi.json"
ibCollectionPlus="./ManuallyScrapedData/IbCollectPlus.json"
IbVCollectPlus = './ManuallyScrapedData/IbVCollectPlus.json'
CentralSchemeToProvideInterestSubsidy="./ManuallyScrapedData/CentralSchemeToProvideInterestSubsidy.json"
CentralizedPensionProcessingSystem = "./ManuallyScrapedData/CentralizedPensionProcessingSystem.json"
image="./ManuallyScrapedData/image.json"
ExecutiveDirectorsProfile="./ManuallyScrapedData/ExecutiveDirectorsProfile.json"
EConfirmationOfBankGuarantee="./ManuallyScrapedData/EConfirmationOfBankGuarantee.json"
ComplaintsOfficersList="./ManuallyScrapedData/ComplaintsOfficersList.json"
IndianBankMutualFund="./ManuallyScrapedData/IndinBankMutualFund.json"
ServiceChargesForexRates="./ManuallyScrapedData/ServiceChargesForex Rates.json"
LendingRates="./ManuallyScrapedData/LendingRates.json"
InternetBanking="./ManuallyScrapedData/InternetBanking.json"
IndPay="./ManuallyScrapedData/IndPay.json"
SupremeCurrentAccounts="./ManuallyScrapedData/SupremeCurrentAccounts.json"
DepositRates="./ManuallyScrapedData/DepositRates.json"
ujjivanBank="./ManuallyScrapedData/ujjivansfb_in_corporate_salary_account.json"

json_files = [
        IbVCollectPlus,
        #ib_surabhi_path,
       # ibCollectionPlus,
       # CentralSchemeToProvideInterestSubsidy,
        #CentralizedPensionProcessingSystem,
        #image,
        #ExecutiveDirectorsProfile,
       # EConfirmationOfBankGuarantee,
        #ComplaintsOfficersList,
#        IndianBankMutualFund,
        #ServiceChargesForexRates,

        #LendingRates,

        InternetBanking,
        #IndPay,
        #SupremeCurrentAccounts,DepositRates
        # ujjivanBank
        #'/home/ubuntuvm/MissionImpossible/indian-bank-v2.2.4/IndianBank-KBOT/generating-json-structure/manual_html_to_json/ManuallyScrapedData/json_v2/ServiceChargesForexRates.json',
        #'/home/ubuntuvm/MissionImpossible/indian-bank-v2.2.4/IndianBank-KBOT/generating-json-structure/manual_html_to_json/ManuallyScrapedData/json_v2/LendingRates.json',
        #'/home/ubuntuvm/MissionImpossible/indian-bank-v2.2.4/IndianBank-KBOT/generating-json-structure/manual_html_to_json/ManuallyScrapedData/json_v2/ind_sme_e_vaahan.json',
        #'/home/ubuntuvm/MissionImpossible/indian-bank-v2.2.4/IndianBank-KBOT/generating-json-structure/manual_html_to_json/ManuallyScrapedData/json_v2/ib_doctor_plus.json',
        #'/home/ubuntuvm/MissionImpossible/indian-bank-v2.2.4/IndianBank-KBOT/generating-json-structure/manual_html_to_json/ManuallyScrapedData/json_v2/IndPay.json',
        #'/home/ubuntuvm/MissionImpossible/indian-bank-v2.2.4/IndianBank-KBOT/generating-json-structure/manual_html_to_json/ManuallyScrapedData/json_v2/general_managers.json'
        ]
documents = []

for json_file in json_files:
        # print('############', json_file, indent=4)
        with open(json_file, "r") as file:
                read_json = json.loads(file.read())
                documents.append(read_json)
               
                print("---------------------------",read_json,"\n \n")



# for url in url_list:
#     document = url_list[url]
#     document['url'] = url
#     document['filename'] = path + url.split('/')[-2] + '/index.html'
print("the documents length is----------",documents.__len__)

def main():
    for document in documents:
        print("^^^^^^^^^^^^^",document)
        rabbitmq_producer.publish(json.dumps(document).encode())


    

rabbitmq_producer = RabbitmqProducerPipe(
        publish_exchange="nlpEx",
        routing_key="nlp",
        queue_name='nlpQueue',
        host="localhost")


if __name__ == "__main__":
    main()
