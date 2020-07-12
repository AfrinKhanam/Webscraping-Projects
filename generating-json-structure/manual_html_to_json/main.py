from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe
import json
import sys
import array as arr

CentralSchemeToProvideInterestSubsidy = "./ManuallyScrapedData/CentralSchemeToProvideInterestSubsidy.json"
CentralizedPensionProcessingSystem = "./ManuallyScrapedData/CentralizedPensionProcessingSystem.json"
image = "./ManuallyScrapedData/image.json"
ExecutiveDirectorsProfile = "./ManuallyScrapedData/ExecutiveDirectorsProfile.json"
EConfirmationOfBankGuarantee = "./ManuallyScrapedData/EConfirmationOfBankGuarantee.json"
ComplaintsOfficersList = "./ManuallyScrapedData/ComplaintsOfficersList.json"
IndianBankMutualFund = "./ManuallyScrapedData/IndinBankMutualFund.json"
ServiceChargesForexRates = "./ManuallyScrapedData/json_v2/ServiceChargesForexRates.json"
LendingRates = "./ManuallyScrapedData/json_v2/LendingRates.json"
InternetBanking = "./ManuallyScrapedData/InternetBanking.json"
IndPay = "./ManuallyScrapedData/json_v2/IndPay.json"
DepositRates = "./ManuallyScrapedData/DepositRates.json"
general_managers = "./ManuallyScrapedData/json_v2/general_managers.json"
cms_plus = "./ManuallyScrapedData/json_v2/cms_plus.json"

json_files = [
    # CentralSchemeToProvideInterestSubsidy,
    CentralizedPensionProcessingSystem,
    # image,
    # ExecutiveDirectorsProfile,
    # EConfirmationOfBankGuarantee,
    # ComplaintsOfficersList,
    # IndianBankMutualFund,
    # ServiceChargesForexRates,
    # LendingRates,
    # InternetBanking,
    # IndPay,
    # DepositRates,
    # cms_plus
]
documents = []

for json_file in json_files:
    with open(json_file, "r") as file:
        read_json = json.loads(file.read())
        documents.append(read_json)
print("the documents length is----------", documents.__len__)

def main():
    for document in documents:
        print("^^^^^^^^^^^^^", json.dumps(document,indent=4))
        rabbitmq_producer.publish(json.dumps(document).encode())


rabbitmq_producer = RabbitmqProducerPipe(
    publish_exchange="nlpEx",
    routing_key="nlp",
    queue_name='nlpQueue',
    host="localhost")


if __name__ == "__main__":
    main()
