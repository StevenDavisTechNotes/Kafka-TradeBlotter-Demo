using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
//using Kafka.Client;
//using Kafka.Client.Cfg;
//using Kafka.Client.Consumers;
//using Kafka.Client.Helper;
//using Kafka.Client.Messages;
//using Kafka.Client.Producers;
//using Kafka.Client.Responses;
//using Kafka.Client.Utils;

namespace KafktaListener
{
    public class KafkaSpoutComplicated
    {
        //public class KafkaEvent
        //{
        //    public long Offset { get; set; }
        //    public string TextKey { get; set; }
        //    public string Text { get; set; }
        //}
        //private CancellationTokenSource cts;

        public KafkaSpoutComplicated(string Topic)
        {
            //cts = new CancellationTokenSource();
            ////MessageSpout = new BlockingCollection<KafkaEvent>(boundedCapacity: 1000);
            //var listenerThread = new Thread(() =>
            //{
            //    ConsumeDataSimple(Topic);
            //    cts.Dispose();
            //})
            //{
            //    Name = $"KafkaListener on {Topic}"
            //};
            //listenerThread.Start();
        }

        //private event Action<KafkaEvent> MessageReceived;
        ////public BlockingCollection<KafkaEvent> MessageSpout { get; private set; }

        //public IObservable<KafkaEvent> WhenMessageReceived
        //{
        //    get
        //    {
        //        return Observable.FromEvent<KafkaEvent>(
        //            h => this.MessageReceived += h,
        //            h => this.MessageReceived -= h);
        //    }
        //}
        //public void Cancel()
        //{
        //    cts.Cancel();
        //}
        #region implementation
        //internal static log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(KafkaSpoutComplicated));

        //private const string ClientID = "KafkaNETLibConsoleConsumer";
        //private const string DumpDataError = "Got ERROR while consume data, please check log file.";
        //private static int correlationID = 0;
        //private static int lastNotifytotalCount = 0;
        //private static int totalCount = 0;

        //internal void ConsumeDataSimple(string Topic, int PartitionIndex = -1, string Offset = "latest", int LastMessagesCount = 10)
        //{
        //    var cancellationToken = cts.Token;
        //    correlationID = 0;
        //    totalCount = 0;
        //    lastNotifytotalCount = 0;
        //    KafkaSimpleManagerConfiguration config = new KafkaSimpleManagerConfiguration()
        //    {
        //        FetchSize = 11 * 1024 * 1024,
        //        BufferSize = 11 * 1024 * 1024,
        //        MaxWaitTime = 0,
        //        MinWaitBytes = 0,
        //        Zookeeper = Properties.Settings.Default.ZookeeperUrl
        //    };
        //    config.Verify();

        //    bool finish = false;
        //    try
        //    {
        //        using (
        //            KafkaSimpleManager<int, Message> kafkaSimpleManager = new KafkaSimpleManager<int, Message>(config))
        //        {
        //            TopicMetadata topicMetadata = kafkaSimpleManager.RefreshMetadata(0, ClientID, correlationID++, Topic,
        //                true);
        //            while (!finish)
        //            {
        //                if (cancellationToken.IsCancellationRequested)
        //                {
        //                    Logger.InfoFormat("Topic:{0} Shutting Down.     totalCount:{1} ", Topic, totalCount);
        //                    return;
        //                }
        //                try
        //                {
        //                    for (int i = 0; i <= topicMetadata.PartitionsMetadata.Max(r => r.PartitionId); i++)
        //                    {
        //                        if (PartitionIndex == -1 || i == PartitionIndex)
        //                        {
        //                            #region Get real offset and adjust

        //                            long earliest = 0;
        //                            long latest = 0;
        //                            long offsetBase = 0;
        //                            OffsetHelper.GetAdjustedOffset<int, Message>(Topic
        //                                , kafkaSimpleManager, i
        //                                , ConvertOffsetType(Offset)
        //                                , ConvertOffset(Offset)
        //                                , LastMessagesCount, out earliest, out latest, out offsetBase);

        //                            #endregion

        //                            //Console.WriteLine(
        //                            //    "Topic:{0} Partition:{1} will read from {2} earliest:{3} latest:{4}", Topic, i,
        //                            //    offsetBase, earliest, latest);
        //                            finish = ConsumeDataOfOnePartition(kafkaSimpleManager, i, offsetBase, earliest,
        //                                latest, cancellationToken, Topic, PartitionIndex, Offset);
        //                            if (finish)
        //                                break;
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    Logger.ErrorFormat("ConsumeDataSimple Got exception, will refresh metadata. {0}",
        //                        ex.FormatException());
        //                    kafkaSimpleManager.RefreshMetadata(0, ClientID, correlationID++, Topic, true);
        //                }
        //            }
        //        }

        //        Logger.InfoFormat("Topic:{0} Finish Read.     totalCount:{1} ", Topic, totalCount);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.ErrorFormat("ConsumeDataSimple  Got exception:{0}\r\nTopic: {1}", ex.FormatException(), Topic);
        //    }
        //    finally
        //    {
        //        //MessageSpout.CompleteAdding();
        //    }
        //}

        //private bool ConsumeDataOfOnePartition<TKey, TData>(KafkaSimpleManager<TKey, TData> kafkaSimpleManager,
        //    int partitionID,
        //    long offsetBase, long earliest, long latest,
        //    CancellationToken cancellationToken,
        //    //BlockingCollection<KafkaEvent> messageSpout,
        //    string Topic,
        //    int PartitionIndex = -1, string Offset = "latest")
        //{
        //    int Count = -1;
        //    Random rand = new Random();
        //    StringBuilder sb = new StringBuilder();

        //    #region repeatly consume and dump data
        //    long offsetLast = -1;
        //    long l = 0;
        //    Logger.InfoFormat("Will read partition {0} from {1}.   Earliese:{2} latest:{3} ", partitionID, offsetBase, earliest, latest);
        //    using (Consumer consumer = kafkaSimpleManager.GetConsumer(Topic, partitionID))
        //    {
        //        while (true)
        //        {
        //            if (cancellationToken.IsCancellationRequested)
        //            {
        //                Logger.InfoFormat("Topic:{0} Partition:{1} Shutting Down.    Earliest:{2} Latest:{3},  totalCount:{4} "
        //                   , Topic, partitionID, earliest, latest, totalCount);
        //                return true;
        //            }
        //            correlationID++;
        //            List<MessageAndOffset> listMessageAndOffsets = FetchAndGetMessageAndOffsetList(consumer
        //                                        , correlationID++,
        //                                        Topic, partitionID, offsetBase,
        //                                        consumer.Config.FetchSize,
        //                                        kafkaSimpleManager.Config.MaxWaitTime,
        //                                        kafkaSimpleManager.Config.MinWaitBytes);

        //            if (listMessageAndOffsets == null)
        //            {
        //                Logger.Error("PullMessage got null  List<MessageAndOffset>, please check log for detail.");
        //                break;
        //            }
        //            else
        //            {

        //                #region dump response.Payload
        //                if (listMessageAndOffsets.Any())
        //                {
        //                    offsetLast = listMessageAndOffsets.Last().MessageOffset;
        //                    totalCount += listMessageAndOffsets.Count;
        //                    foreach (var v in listMessageAndOffsets)
        //                    {
        //                        var convertedMsg = new KafkaEvent()
        //                        {
        //                            Offset = v.Message.Offset,
        //                            TextKey = v.Message.Key==null?"":Encoding.UTF8.GetString(v.Message.Key),
        //                            Text = Encoding.UTF8.GetString(v.Message.Payload)
        //                        };
        //                        //messageSpout.Add(convertedMsg, cancellationToken);
        //                        MessageReceived?.Invoke(convertedMsg);
        //                    }
        //                    Logger.InfoFormat("Finish read partition {0} to {1}.   Earliese:{2} latest:{3} ", partitionID, offsetLast, earliest, latest);
        //                    offsetBase = offsetLast + 1;
        //                    if (totalCount - lastNotifytotalCount > 1000)
        //                    {
        //                        //Console.WriteLine("Partition: {0} totally read  {1}  will continue read from   {2}", partitionID, totalCount, offsetBase);
        //                        lastNotifytotalCount = totalCount;
        //                    }
        //                }
        //                else
        //                {
        //                    Logger.InfoFormat("Finish read partition {0} to {1}.   Earliese:{2} latest:{3} ", partitionID, offsetLast, earliest, latest);
        //                    //Console.WriteLine("Partition: {0} totally read  {1}  Hit end of queue   {2}", partitionID, totalCount, offsetBase);
        //                    break;
        //                }
        //                Thread.Sleep(1000);
        //                #endregion
        //            }
        //            l++;
        //            if (totalCount >= Count && Count > 0)
        //                return true;
        //        }
        //    }
        //    #endregion
        //    Logger.InfoFormat("Topic:{0} Partition:{1} Finish Read.    Earliest:{2} Latest:{3},  totalCount:{4} "
        //                   , Topic, partitionID, earliest, latest, totalCount);

        //    if (totalCount >= Count && Count > 0)
        //        return true;
        //    else
        //        return false;
        //}

        //private static List<MessageAndOffset> FetchAndGetMessageAndOffsetList(
        //    Consumer consumer,
        //    int correlationID,
        //    string topic,
        //    int partitionIndex,
        //    long fetchOffset,
        //    int fetchSize,
        //    int maxWaitTime,
        //    int minWaitSize)
        //{
        //    List<MessageAndOffset> listMessageAndOffsets = new List<MessageAndOffset>();
        //    PartitionData partitionData = null;
        //    int payloadCount = 0;
        //    // at least retry once
        //    int maxRetry = 1;
        //    int retryCount = 0;
        //    string s = string.Empty;
        //    bool success = false;
        //    while (!success && retryCount < maxRetry)
        //    {
        //        try
        //        {
        //            FetchResponse response = consumer.Fetch(Assembly.GetExecutingAssembly().ManifestModule.ToString(),      // client id
        //                topic,
        //                correlationID, //random.Next(int.MinValue, int.MaxValue),                        // correlation id
        //                partitionIndex,
        //                fetchOffset,
        //                fetchSize,
        //                maxWaitTime,
        //                minWaitSize);

        //            if (response == null)
        //            {
        //                throw new KeyNotFoundException(string.Format("FetchRequest returned null response,fetchOffset={0},leader={1},topic={2},partition={3}",
        //                    fetchOffset, consumer.Config.Broker, topic, partitionIndex));
        //            }

        //            partitionData = response.PartitionData(topic, partitionIndex);
        //            if (partitionData == null)
        //            {
        //                throw new KeyNotFoundException(string.Format("PartitionData is null,fetchOffset={0},leader={1},topic={2},partition={3}",
        //                    fetchOffset, consumer.Config.Broker, topic, partitionIndex));
        //            }

        //            if (partitionData.Error == ErrorMapping.OffsetOutOfRangeCode)
        //            {
        //                s = "PullMessage OffsetOutOfRangeCode,change to Latest,topic={0},leader={1},partition={2},FetchOffset={3},retryCount={4},maxRetry={5}";
        //                Logger.ErrorFormat(s, topic, consumer.Config.Broker, partitionIndex, fetchOffset, retryCount, maxRetry);
        //                return null;
        //            }

        //            if (partitionData.Error != ErrorMapping.NoError)
        //            {
        //                s = "PullMessage ErrorCode={0},topic={1},leader={2},partition={3},FetchOffset={4},retryCount={5},maxRetry={6}";
        //                Logger.ErrorFormat(s, partitionData.Error, topic, consumer.Config.Broker, partitionIndex, fetchOffset, retryCount, maxRetry);
        //                return null;
        //            }

        //            success = true;
        //            listMessageAndOffsets = partitionData.GetMessageAndOffsets();
        //            if (null != listMessageAndOffsets && listMessageAndOffsets.Any())
        //            {
        //                //TODO: When key are same for sequence of message, need count payloadCount by this way.  So why line 438 work? is there bug?
        //                payloadCount = listMessageAndOffsets.Count;// messages.Count();

        //                long lastOffset = listMessageAndOffsets.Last().MessageOffset;

        //                if ((payloadCount + fetchOffset) != (lastOffset + 1))
        //                {
        //                    s = "PullMessage offset payloadCount out-of-sync,topic={0},leader={1},partition={2},payloadCount={3},FetchOffset={4},lastOffset={5},retryCount={6},maxRetry={7}";
        //                    Logger.ErrorFormat(s, topic, consumer.Config.Broker, partitionIndex, payloadCount, fetchOffset, lastOffset, retryCount, maxRetry);
        //                }
        //            }

        //            return listMessageAndOffsets;
        //        }
        //        catch (Exception)
        //        {
        //            if (retryCount >= maxRetry)
        //            {
        //                throw;
        //            }
        //        }
        //        finally
        //        {
        //            retryCount++;
        //        }
        //    } // end of while loop

        //    return listMessageAndOffsets;
        //}

        //internal static KafkaOffsetType ConvertOffsetType(string offset)
        //{
        //    KafkaOffsetType offsetType = KafkaOffsetType.Earliest;

        //    if (string.IsNullOrEmpty(offset))
        //    {
        //        throw new ArgumentNullException("offset");
        //    }

        //    switch (offset.ToLower(CultureInfo.InvariantCulture))
        //    {
        //        case "earliest":
        //            offsetType = KafkaOffsetType.Earliest;
        //            break;
        //        case "latest":
        //            offsetType = KafkaOffsetType.Latest;
        //            break;
        //        case "last":
        //            offsetType = KafkaOffsetType.Last;
        //            break;
        //        default:
        //            offsetType = KafkaOffsetType.Timestamp;
        //            break;
        //    }

        //    return offsetType;
        //}

        //internal static long ConvertOffset(string offset)
        //{
        //    long offsetTime = 0;
        //    bool success = false;

        //    if (string.IsNullOrEmpty(offset))
        //    {
        //        throw new ArgumentNullException("offset");
        //    }

        //    switch (offset.ToLower(CultureInfo.InvariantCulture))
        //    {
        //        case "earliest":
        //        case "latest":
        //        case "last":
        //            offsetTime = 0;
        //            break;
        //        default:
        //            DateTime dateTimeOffset;
        //            if (DateTime.TryParse(offset, out dateTimeOffset))
        //            {
        //                offsetTime = KafkaClientHelperUtils.ToUnixTimestampMillis(dateTimeOffset);
        //                success = true;
        //            }
        //            else if (long.TryParse(offset, out offsetTime))
        //            {
        //                success = true;
        //            }

        //            if (!success)
        //            {
        //                Logger.Error(string.Format("Error: invalid offset={0}, it should be either earliest|latest|last or an unsigned integer or a timestamp.", offset));
        //                throw new ArgumentException(string.Format("invalid offset={0}", offset));
        //            }

        //            break;
        //    }

        //    return offsetTime;
        //}



        #endregion
    }
}