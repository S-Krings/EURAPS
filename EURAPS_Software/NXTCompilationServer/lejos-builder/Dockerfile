
FROM mcr.microsoft.com/java/jdk:8-zulu-ubuntu
RUN apt-get update -y && apt-get upgrade -y
RUN apt-get install -y curl
WORKDIR /root/lejos
RUN curl https://kumisystems.dl.sourceforge.net/project/nxt.lejos.p/0.9.1beta-3/leJOS_NXJ_0.9.1beta-3.tar.gz --output leJOS_NXJ_0.9.1beta-3.tar.gz
RUN tar -xvf leJOS_NXJ_0.9.1beta-3.tar.gz
ENV PATH /root/lejos/leJOS_NXJ_0.9.1beta-3/bin:$PATH
ENV NXJ_HOME /root/lejos/leJOS_NXJ_0.9.1beta-3

ENV Main HalloWelt
ENV Namespace test2

WORKDIR /root/build
ENTRYPOINT [ "sh", "-c", "nxjc $Namespace/$Main.java && nxjlink -o $Main.nxj $Namespace.$Main" ]
