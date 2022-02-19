
const express = require('express')
var bodyParser = require('body-parser')
const { exec } = require('child_process');
const fs = require("fs")
const app = express()
app.use(bodyParser.urlencoded({ extended: false }))
app.use(bodyParser.json())
const port = 3000

app.post('/api/compile', (req, res) => {
    // create folder by timestamp to allow parallel builds
    const folder = new Date().getTime();
    if(!fs.existsSync(`files/${folder}/${req.body.namespace}/`)){
      fs.mkdirSync(`files/${folder}/${req.body.namespace}/`,{recursive:true});
    }
    fs.writeFileSync(`files/${folder}/job.yml`,`
    apiVersion: batch/v1
    kind: Job
    metadata:
      name: lejosbuilder${folder}
    spec:
      ttlSecondsAfterFinished: 10
      template:
        spec:
          volumes:
            - name: build${folder}
              hostPath:
                path: /host/${folder}
                type: Directory
          containers:
          - name: lejosbuilder${folder}
            image: lejosbuilder
            imagePullPolicy: Never
            volumeMounts:
            - name: build${folder}
              mountPath: /root/build
          restartPolicy: Never
    `)
    fs.writeFileSync(`files/${folder}/${req.body.namespace}/${req.body.classname}.java`, req.body.code)
    //exec(`docker run -v "${__dirname}/files/${folder}:/root/build" lejosbuilder`, (err, sysout, stderr) => {
    exec(`kubectl apply -f files/${folder}/job.yml && kubectl wait --for=condition=complete job/lejosbuilder${folder}`, (err, sysout, stderr) => {
      console.log("finished build")
      if(err){
        console.log(err);
      }
      if(stderr){
        console.log(stderr);
      }
      console.log(sysout);
      const compiledData = fs.readFileSync(`files/${folder}/${req.body.classname}.nxj`)
      /*fs.unlink(`files/${folder}/${req.body.classname}.nxj`, () => {});
      fs.unlink(`files/${folder}/${req.body.namespace}/${req.body.classname}.java`, () => {});
      fs.unlink(`files/${folder}/${req.body.namespace}/${req.body.classname}.class`, () => {});
      fs.rmdir(`files/${folder}/${req.body.namespace}`, () => {});
      fs.rmdir(`files/${folder}`, () => {});*/
      res.send(compiledData);
      exec(`kubectl delete job lejosbuilder${folder}`, (err, sysout, stderr) => {
        console.log("job purged after finished");
      })
    });
})

app.listen(port, () => {
  console.log(`Example app listening at http://localhost:${port}`)
})