// AttachmentModify  
// ----------------------------------------------------------  
// Example for intercepting email messages in an Exchange 2010 transport queue  
//   
// The example intercepts messages sent from a configurable email address(es)  
// and checks the mail message for attachments have filename in to format  
//   
//      WORKBOOK_{GUID}  
//  
// Changing the filename of the attachments makes it easier for the information worker  
// to identify the reports in the emails and in the file system as well.  
//  
// ----------------------------------------------------------  

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

// the lovely Exchange   
using Microsoft.Exchange.Data.Transport;
using Microsoft.Exchange.Data.Transport.Smtp;
using Microsoft.Exchange.Data.Transport.Email;
using Microsoft.Exchange.Data.Transport.Routing;

namespace QuanShi.MessageModify
{
    #region Message Modifier Factory

    /// <summary>  
    /// Message Modifier Factory  
    /// </summary>  
    public class MessageModifierFactory : RoutingAgentFactory
    {

        public override RoutingAgent CreateAgent(SmtpServer server)
        {
            return new MessageModifier();
        }
    }  

    #endregion

    #region Message Modifier Routing Agent

    /// <summary>  
    /// The Message Modifier Routing Agent for modifying an email message  
    /// </summary>  
    public class MessageModifier : RoutingAgent
    {
        // The agent uses the fileLock object to synchronize access to the log file  
        private object fileLock = new object();

        /// <summary>  
        /// The current MailItem the transport agent is handling  
        /// </summary>  
        private MailItem mailItem;

        /// <summary>  
        /// This context to allow Exchange to continue processing a message  
        /// </summary>  
        private AgentAsyncContext agentAsyncContext;

        public MessageModifier()
        {
            this.OnRoutedMessage += OnRoutedMessageHandler;
        }  
  

        /// <summary>  
        /// Event handler for OnRoutedMessage event  
        /// </summary>  
        /// <param name="source">Routed Message Event Source</param>  
        /// <param name="args">Queued Message Event Arguments</param>  
        void OnRoutedMessageHandler(RoutedMessageEventSource source, QueuedMessageEventArgs args)
        {
            lock (fileLock)
            {
                try
                {
                    this.mailItem = args.MailItem;
                    this.agentAsyncContext = this.GetAgentAsyncContext();  

                    foreach(EmailRecipient rec in this.mailItem.Message.To)
                    {
                        if (rec.NativeAddress == "ex10test04@hitest10.com")
                        {
                            Meeting meeting = new Meeting();
                            meeting.Create();
                        }
                    }
                          
                          
                }
                catch (System.IO.IOException ex)
                {
                   
                    Debug.WriteLine(ex.ToString());
                    this.agentAsyncContext.Complete();
                }
                finally
                {
                    // We are done  
                    this.agentAsyncContext.Complete();
                }
            }

            // Return to pipeline  
            return;
        }

    }

    #endregion

}