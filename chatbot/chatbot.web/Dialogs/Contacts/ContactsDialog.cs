using System;
using System.Collections.Generic;
using System.Linq;

using UjjivanBank_ChatBOT.Dialogs.Main;

using Microsoft.Bot.Builder;

namespace UjjivanBank_ChatBOT.Dialogs.Contacts
{
    public class ContactsDialog
    {
        #region Properties

        private static ContactsResponse _contactsResponder = new ContactsResponse();
        private static MainResponses _responder = new MainResponses();
        public static ContactsResponse.ContactData _contactsData = new ContactsResponse.ContactData();

        #endregion

        #region Methods

        /// <summary>
        /// Builds the rates sub menu card.
        /// </summary>
        /// <param name="turnContext">The turn context.</param>
        /// <param name="result">The result.</param>
        public static async void BuildContactsSubMenuCard(ITurnContext turnContext, RecognizerResult result)
        {
            try
            {
                string entityName = string.Empty;
                string entityType = string.Empty;
                List<string> entityTypes = new List<string>
                {
                    "contacts_entity",
                    "customer_support_entity",
                    "email_id_entity"
                };

                foreach (var entity in entityTypes)
                {
                    if (result.Entities[entity] != null)
                    {
                        entityType = entity;
                        entityName = (string)result.Entities[entity].Values<string>().FirstOrDefault();
                    }
                }

                if (entityType.Equals("contacts_entity"))
                {
                    switch (entityName)
                    {
                        case ContactEntities.QuickContacts:
                            {
                                _contactsData = getContactData(ContactEntities.QuickContacts);
                                await _contactsResponder.ReplyWith(turnContext, ContactsResponse.ContactResponseIds.QuickContactsDisplay, _contactsData);
                                break;
                            }
                        case ContactEntities.CustomerSupport:
                            {
                                await turnContext.SendActivityAsync("Please find the types of Customer Support.\nSelect from the menu to proceed further.");
                                await _contactsResponder.ReplyWith(turnContext, ContactsResponse.ContactResponseIds.CustomerSupportDisplay);
                                break;
                            }
                        case ContactEntities.EmailIDs:
                            {
                                await turnContext.SendActivityAsync("Please find that the following Bank Email ID's available.\nSelect from the menu to proceed further.");
                                await _contactsResponder.ReplyWith(turnContext, ContactsResponse.ContactResponseIds.EmailIDDisplay);
                                break;
                            }
                        default:
                            {
                                await turnContext.SendActivityAsync("Sorry, I didn't understand. Please try with different query");
                                break;
                            }
                    }
                }
                else if (entityType.Equals("customer_support_entity"))
                {
                    switch (entityName)
                    {
                        case ContactEntities.CustomerComplaints:
                            {
                                _contactsData = getContactData(ContactEntities.CustomerComplaints);
                                await _contactsResponder.ReplyWith(turnContext, ContactsResponse.ContactResponseIds.CustomerComplaintsDisplay, _contactsData);
                                break;
                            }
                        case ContactEntities.ComplaintsOfficers:
                            {
                                _contactsData = getContactData(ContactEntities.ComplaintsOfficers);
                                await _contactsResponder.ReplyWith(turnContext, ContactsResponse.ContactResponseIds.ComplaintsOfficersListDisplay, _contactsData);
                                break;
                            }
                        case ContactEntities.ChiefVigilanceOfficer:
                            {
                                _contactsData = getContactData(ContactEntities.ChiefVigilanceOfficer);
                                await _contactsResponder.ReplyWith(turnContext, ContactsResponse.ContactResponseIds.ChiefVigilanceOfficerDisplay, _contactsData);
                                break;
                            }
                        default:
                            {
                                await turnContext.SendActivityAsync("Sorry, I didn't understand. Please try with different query");
                                break;
                            }
                    }
                }
                else if (entityType.Equals("email_id_entity"))
                {
                    switch (entityName)
                    {
                        case ContactEntities.HeadOffice:
                            {
                                _contactsData = getContactData(ContactEntities.HeadOffice);
                                await _contactsResponder.ReplyWith(turnContext, ContactsResponse.ContactResponseIds.HeadOfficeEmailIDDisplay, _contactsData);
                                break;
                            }
                        case ContactEntities.Department:
                            {
                                _contactsData = getContactData(ContactEntities.Department);
                                await _contactsResponder.ReplyWith(turnContext, ContactsResponse.ContactResponseIds.DepartmentEmailIDDisplay, _contactsData);
                                break;
                            }
                        case ContactEntities.Executives:
                            {
                                _contactsData = getContactData(ContactEntities.Executives);
                                await _contactsResponder.ReplyWith(turnContext, ContactsResponse.ContactResponseIds.ExecutivesEmailIDDisplay, _contactsData);
                                break;
                            }
                        case ContactEntities.IMAGE:
                            {
                                _contactsData = getContactData(ContactEntities.IMAGE);
                                await _contactsResponder.ReplyWith(turnContext, ContactsResponse.ContactResponseIds.IMAGEEmailIDDisplay, _contactsData);
                                break;
                            }
                        case ContactEntities.ForeginBranches:
                            {
                                _contactsData = getContactData(ContactEntities.ForeginBranches);
                                await _contactsResponder.ReplyWith(turnContext, ContactsResponse.ContactResponseIds.ForeginBranchesEmailIDDisplay, _contactsData);
                                break;
                            }
                        case ContactEntities.OverseasBranches:
                            {
                                _contactsData = getContactData(ContactEntities.OverseasBranches);
                                await _contactsResponder.ReplyWith(turnContext, ContactsResponse.ContactResponseIds.OverseasBranchesEmailIDDisplay, _contactsData);
                                break;
                            }
                        case ContactEntities.NRIBranches:
                            {
                                _contactsData = getContactData(ContactEntities.NRIBranches);
                                await _contactsResponder.ReplyWith(turnContext, ContactsResponse.ContactResponseIds.NRIBranchesEmailIDDisplay, _contactsData);
                                break;
                            }
                        case ContactEntities.ZonalOffices:
                            {
                                _contactsData = getContactData(ContactEntities.ZonalOffices);
                                await _contactsResponder.ReplyWith(turnContext, ContactsResponse.ContactResponseIds.ZonalOfficesEmailIDDisplay, _contactsData);
                                break;
                            }
                        case ContactEntities.EConformationOfBankGuarantee:
                            {
                                _contactsData = getContactData(ContactEntities.EConformationOfBankGuarantee);
                                await _contactsResponder.ReplyWith(turnContext, ContactsResponse.ContactResponseIds.EConformationOfBankGuaranteeEmailIDDisplay, _contactsData);
                                break;
                            }
                        default:
                            {
                                await turnContext.SendActivityAsync("Sorry, I didn't understand. Please try with different query");
                                break;
                            }
                    }
                }
                else
                {
                    await turnContext.SendActivityAsync("Please find the types of Contacts.\n Select any Contact type to proceed further.");
                    await _responder.ReplyWith(turnContext, MainResponses.ResponseIds.ContactsMenuCardDisplay);

                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        public static ContactsResponse.ContactData getContactData(string result)
        {
            ContactsResponse.ContactData contactsData = new ContactsResponse.ContactData();
            var res = ContactsResponse.keyValuePairs.TryGetValue(result, out contactsData);
            return contactsData;
        }

        #endregion
    }
}
