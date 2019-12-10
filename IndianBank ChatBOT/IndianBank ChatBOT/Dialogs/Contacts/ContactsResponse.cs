using IndianBank_ChatBOT.Dialogs.Shared;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.TemplateManager;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Dialogs.Contacts
{
    public class ContactsResponse : TemplateManager
    {
        #region Properties

        private static LanguageTemplateDictionary _responseTemplates = new LanguageTemplateDictionary
        {
            ["default"] = new TemplateIdMap
            {
                //Contacts Menu Card
                {ContactResponseIds.QuickContactsDisplay,(context, data) => BuildContactCardDisplay(context,data) },
                {ContactResponseIds.CustomerSupportDisplay,(context, data) => CustomerSupportMenuCardDisplay(context) },
                {ContactResponseIds.EmailIDDisplay,(context, data) => EmailIDMenuCardDisplay(context) },

                //Customer Support Menu Card
                {ContactResponseIds.CustomerComplaintsDisplay,(context, data) => BuildContactCardDisplay(context,data) },
                {ContactResponseIds.ComplaintsOfficersListDisplay,(context, data) => BuildContactCardDisplay(context,data) },
                {ContactResponseIds.ChiefVigilanceOfficerDisplay,(context, data) => BuildContactCardDisplay(context,data) },

                //Email IDs Menu Card
                {ContactResponseIds.HeadOfficeEmailIDDisplay,(context, data) => BuildContactCardDisplay(context,data) },
                {ContactResponseIds.DepartmentEmailIDDisplay,(context, data) => BuildContactCardDisplay(context,data) },
                {ContactResponseIds.ExecutivesEmailIDDisplay,(context, data) => BuildContactCardDisplay(context,data) },
                {ContactResponseIds.IMAGEEmailIDDisplay,(context, data) => BuildContactCardDisplay(context,data) },
                {ContactResponseIds.ForeginBranchesEmailIDDisplay,(context, data) => BuildContactCardDisplay(context,data) },
                {ContactResponseIds.OverseasBranchesEmailIDDisplay,(context, data) => BuildContactCardDisplay(context,data) },
                {ContactResponseIds.NRIBranchesEmailIDDisplay,(context, data) => BuildContactCardDisplay(context,data) },
                {ContactResponseIds.ZonalOfficesEmailIDDisplay,(context, data) => BuildContactCardDisplay(context,data) },
                {ContactResponseIds.EConformationOfBankGuaranteeEmailIDDisplay,(context, data) => BuildContactCardDisplay(context,data) }
            }
        };

        public static Dictionary<string,ContactData> keyValuePairs = new Dictionary<string, ContactData>
        {
            //Contacts Menu Items
            {"quick contact",new ContactData{ContactDataTitle="Quick Contacts",ContactDataText="To get the contact details. Click on Read More",ContactDataLink=ContactResponseLinks.QuickContacts,ContactDataImagePath=Path.Combine(".",@"Resources\contacts\QuickContacts", "quick_contacts.PNG")} },

            //Customer Support Menu Items
            {"customer complaints",new ContactData{ContactDataTitle="Customer Complaints",ContactDataText="To submit any complaints please click on Read More",ContactDataLink=CustomerSupportResponseLinks.CustomerComplaints,ContactDataImagePath=Path.Combine(".",@"Resources\contacts\CustomerSupport", "customer_complaints.PNG")} },
            {"complaints officers",new ContactData{ContactDataTitle="Complaint Officers List",ContactDataText="To know the details of the complaint officers. Please click on Read More",ContactDataLink=CustomerSupportResponseLinks.ComplaintOfficersList,ContactDataImagePath=Path.Combine(".",@"Resources\contacts\CustomerSupport", "complaints_officers_list.PNG")} },
            {"chief vigilance officer",new ContactData{ContactDataTitle="Chief Vigilance Officer",ContactDataText="To know about the Chief Vigilance officer. Please click on Read More",ContactDataLink=CustomerSupportResponseLinks.ChiefVigilanceOfficer,ContactDataImagePath=Path.Combine(".",@"Resources\contacts\CustomerSupport", "chief_vigilance_officer.PNG")} },

            //Email ID Menu Items
            {"head office",new ContactData{ContactDataTitle="Head Office",ContactDataText="To get the email ID of the head office. Click on Read More ",ContactDataLink=EmailIDsResponseLinks.HeadOffice,ContactDataImagePath=Path.Combine(".",@"Resources\contacts\EmailIDs", "head_office.PNG")} },
            {"department",new ContactData{ContactDataTitle="Department",ContactDataText="To get the email ID of all the departments. Click on Read More",ContactDataLink=EmailIDsResponseLinks.Department,ContactDataImagePath=Path.Combine(".",@"Resources\contacts\EmailIDs", "department.PNG")} },
            {"executives",new ContactData{ContactDataTitle="Executives",ContactDataText="To get the email ID of the executives. Click on Read More",ContactDataLink=EmailIDsResponseLinks.Executives,ContactDataImagePath=Path.Combine(".",@"Resources\contacts\EmailIDs", "executives.PNG")} },
            {"image",new ContactData{ContactDataTitle="IMAGE",ContactDataText="To get the email ID of the IMAGE. Click on Read More",ContactDataLink=EmailIDsResponseLinks.IMAGE,ContactDataImagePath=Path.Combine(".",@"Resources\contacts\EmailIDs", "image.PNG")} },
            {"foregin branches",new ContactData{ContactDataTitle="Foreign Branches",ContactDataText="To get the email ID of Foreign Branches. Click on Read More",ContactDataLink=EmailIDsResponseLinks.ForeignBranches,ContactDataImagePath=Path.Combine(".",@"Resources\contacts\EmailIDs", "foreign_branches.PNG")} },
            {"overseas branches",new ContactData{ContactDataTitle="Overseas Branches",ContactDataText="To get the email ID of Overseas Branches. Click on Read More",ContactDataLink=EmailIDsResponseLinks.OverseasBranches,ContactDataImagePath=Path.Combine(".",@"Resources\contacts\EmailIDs", "overseas_branches.PNG")} },
            {"nri branches",new ContactData{ContactDataTitle="NRI Branches",ContactDataText="To get the email ID of NRI Branches. Click on Read More",ContactDataLink=EmailIDsResponseLinks.NRIBranches,ContactDataImagePath=Path.Combine(".",@"Resources\contacts\EmailIDs", "nri_branches.PNG")} },
            {"zonal offices",new ContactData{ContactDataTitle="Zonal Offices",ContactDataText="To get the email ID of Zonal Branches. Click on Read More",ContactDataLink=EmailIDsResponseLinks.ZonalOffices,ContactDataImagePath=Path.Combine(".",@"Resources\contacts\EmailIDs", "zonal_offices.PNG")} },
            {"conformation of bank guarantee",new ContactData{ContactDataTitle="E Conformation of Bank Guarantee",ContactDataText="To get the details of eConformation of bank guarantee. Click on Read More",ContactDataLink=EmailIDsResponseLinks.EConformationOfBankGuarantee,ContactDataImagePath=Path.Combine(".",@"Resources\contacts\EmailIDs", "econfirmation_of_bank_guarantee.PNG")} },

        };

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactsResponses"/> class.
        /// </summary>
        public ContactsResponse()
        {
            Register(new DictionaryRenderer(_responseTemplates));
        }

        #endregion

        #region Methods

        private static object BuildContactCardDisplay(ITurnContext context, ContactData data)
        {
            var attachment = new HeroCard()
            {
                Title = data.ContactDataTitle,
                Text = data.ContactDataText,
                Images = new List<CardImage> { new CardImage(data.ContactDataImagePath) },
                Buttons = new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.OpenUrl, title: "Read More", value: data.ContactDataLink)
                }
            }.ToAttachment();

            var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
            return response;
        }

        public static IMessageActivity CustomerSupportMenuCardDisplay(ITurnContext turnContext)
        {
            var attachment = new HeroCard()
            {
                Buttons = SharedResponses.SuggestedActionsForCustomerSupport.Actions
            }.ToAttachment();

            var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
            return response;
        }

        public static IMessageActivity EmailIDMenuCardDisplay(ITurnContext turnContext)
        {
            var attachment = new HeroCard()
            {
                Buttons = SharedResponses.SuggestedActionsForEmailIDs.Actions
            }.ToAttachment();

            var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
            return response;
        }

        #endregion

        #region Class

        public class ContactResponseIds
        {
            //Contacts Constants
            public const string QuickContactsDisplay = "quickContactsCardDisplay";
            public const string CustomerSupportDisplay = "customerSupportCardDisplay";
            public const string EmailIDDisplay = "emailIdCardDisplay";

            //Customer Support Constants
            public const string CustomerComplaintsDisplay = "customerComplaintCardDisplay";
            public const string ComplaintsOfficersListDisplay = "complaintsOfficersListCardDisplay";
            public const string ChiefVigilanceOfficerDisplay = "chiefVigilanceOfficerCardDisplay";

            //Email ID's Constants
            public const string HeadOfficeEmailIDDisplay = "headOfficeEmailIDCardDisplay";
            public const string DepartmentEmailIDDisplay = "departmentEmailIDCardDisplay";
            public const string ExecutivesEmailIDDisplay = "executivesEmailIDCardDisplay";
            public const string IMAGEEmailIDDisplay = "imageEmailIDCardDisplay";
            public const string ForeginBranchesEmailIDDisplay = "foreginBranchesEmailIDCardDisplay";
            public const string OverseasBranchesEmailIDDisplay = "overseasBranchesEmailIDCardDisplay";
            public const string NRIBranchesEmailIDDisplay = "nriBranchesEmailIDCardDisplay";
            public const string ZonalOfficesEmailIDDisplay = "zonalOfficesEmailIDCardDisplay";
            public const string EConformationOfBankGuaranteeEmailIDDisplay = "eConformationOfBankGuaranteeCardDisplay";

        }

        public class ContactResponseLinks
        {
            // Links
            public const string QuickContacts = "https://www.indianbank.in/departments/quick-contact/#!";
        }

        public class CustomerSupportResponseLinks
        {
            public const string CustomerComplaints = "https://www.indianbank.in/customer-complaints/#!";
            public const string ComplaintOfficersList = "https://www.indianbank.in/departments/complaints-officers-list/#!";
            public const string ChiefVigilanceOfficer = "https://www.indianbank.in/departments/chief-vigilance-officer/#!";
        }

        public class EmailIDsResponseLinks
        {
            public const string HeadOffice = "https://www.indianbank.in/head-office/#!";
            public const string Department = "https://www.indianbank.in/department/#!";
            public const string Executives = "https://www.indianbank.in/executives/#!";
            public const string IMAGE = "https://www.indianbank.in/image/#!";
            public const string ForeignBranches = "https://www.indianbank.in/foreign-branches/#!";
            public const string OverseasBranches = "https://www.indianbank.in/overseas-branches/#!";
            public const string NRIBranches = "https://www.indianbank.in/nri-branches/#!";
            public const string ZonalOffices = "https://www.indianbank.in/zonal-offices/#!";
            public const string EConformationOfBankGuarantee = "https://www.indianbank.in/e-confirmation-of-bank-guarantee/#!";
        }


        public class ContactData
        {
            public string ContactDataTitle { get; set; }
            public string ContactDataText { get; set; }
            public string ContactDataLink { get; set; }
            public string ContactDataImagePath { get; set; }
        }

        #endregion
    }
}
