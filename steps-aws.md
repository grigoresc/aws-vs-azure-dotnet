
# local computer
```bash
docker-compose up -d
```
or
```bash
docker run -d -it -p 127.0.0.1:4566:4566   -p 127.0.0.1:4510-4559:4510-4559   -v /var/run/docker.sock:/var/run/docker.sock   localstack/localstack:4.0.3
```

```bash
pip install awscli

pip install awscli-local

```

# infra/buidl/deploy:
from https://stackoverflow.com/questions/66551987/how-to-run-aws-lambda-dotnet-on-localstack

```bash

awslocal iam create-role --role-name lambda-dotnet-ex --assume-role-policy-document '{"Version": "2012-10-17", "Statement": [{ "Effect": "Allow", "Principal": {"Service": "lambda.amazonaws.com"}, "Action": "sts:AssumeRole"}]}'

awslocal iam attach-role-policy --role-name lambda-dotnet-ex --policy-arn arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole

dotnet publish -c Release -o publish -p:PublishReadyToRun=false

cd publish 

zip -r ../function.zip *

cd ..

awslocal lambda create-function --function-name lambda-dotnet-function2 --zip-file fileb://function.zip --handler AWSLambda2::AWSLambda2.Function::FunctionHandler --runtime dotnet8 --role arn:aws:iam::000000000000:role/lambda-dotnet-ex

awslocal lambda list-functions

awslocal lambda invoke --function-name lambda-dotnet-function2 --payload "\"Just Checking If Everything is OK again\"" response.json --log-type Tail
```

create bucket (and upload an image to it)

```bash
awslocal s3api create-bucket --bucket sample-bucket
awslocal s3api list-buckets
awslocal s3api put-object \
  --bucket sample-bucket \
  --key image.jpg \
  --body ../assets/image.jpg
```

trigger lambdas by s3 events

```bash
awslocal lambda add-permission --function-name lambda-dotnet-function2 \
--principal s3.amazonaws.com --statement-id SomeID \
--action "lambda:InvokeFunction" --source-arn arn:aws:s3:::sample-bucket \
--source-account account-id

awslocal s3api put-bucket-notification-configuration --bucket sample-bucket \
--notification-configuration '{
  "LambdaFunctionConfigurations": [
    {
      "LambdaFunctionArn": "arn:aws:lambda:us-east-1:000000000000:function:lambda-dotnet-function2",
      "Events": ["s3:ObjectCreated:*"]
    }
  ]
}'
```

# csproj changes:
<PublishReadyToRun>false</PublishReadyToRun>


# queue

```

awslocal sqs create-queue --queue-name MyQueue

awslocal sqs list-queues

awslocal iam put-role-policy --role-name lambda-dotnet-ex --policy-name SQS-SendMessage-Policy --policy-document file://lambda-policy.json
```


```
dotnet publish -c Release -o publish -p:PublishReadyToRun=false

cd publish 

zip -r ../function.zip *

cd ..

awslocal lambda update-function-code --function-name lambda-dotnet-function2 --zip-file fileb://function.zip

awslocal lambda list-functions

awslocal lambda invoke --function-name lambda-dotnet-function2 --payload "\"Just Checking If Everything is OK again\"" response.json --log-type Tail
```
