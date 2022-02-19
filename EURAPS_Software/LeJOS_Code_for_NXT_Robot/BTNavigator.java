package com.prakhar.speared;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;

import lejos.geom.Point;
import lejos.nxt.LCD;
import lejos.nxt.Motor;
import lejos.nxt.comm.BTConnection;
import lejos.nxt.comm.Bluetooth;
import lejos.nxt.comm.NXTConnection;
import lejos.robotics.RegulatedMotor;
import lejos.robotics.localization.OdometryPoseProvider;
import lejos.robotics.navigation.ArcRotateMoveController;
import lejos.robotics.navigation.DifferentialPilot;
import lejos.robotics.navigation.Pose;
import lejos.util.PilotProps;

public class BTNavigator {
					
	private ArcRotateMoveController classPilotForward;
	
	private OdometryPoseProvider poseProviderForward;
	
	BTConnection connection;
	static DataInputStream dataInputStream;
	static DataOutputStream dataOutputStream;
	static BTConnection btc;
	
	static BTNavigator backwardNav;
	public static final int FORWARD = 1;
	public static final int BACKWARD = 2;
	
	static DifferentialPilot backPilot;
	static BTNavigator forwardNav;
	//for display
	static int onLCDPositionY = 0;
				
	private static float WHEEL_DIAMETER = 5.5f;
	private static float WHEEL_DISTANCE = 10.0f;
	
	public BTNavigator(final ArcRotateMoveController pilot, int direction) {
		classPilotForward = pilot;
		poseProviderForward = new OdometryPoseProvider(classPilotForward);
		classPilotForward.setTravelSpeed(25);
		classPilotForward.setRotateSpeed(90);
	}
	
	public static void main(String...strings)  {
		
		while(true) {
			
			connectTheDevice();
			
			try {
			 	
				PilotProps pp = new PilotProps();
				
				pp.setProperty(PilotProps.KEY_WHEELDIAMETER, "" + WHEEL_DIAMETER);
				pp.setProperty(PilotProps.KEY_TRACKWIDTH, "" + WHEEL_DISTANCE);
				
		    	pp.loadPersistentValues();
		    	float wheelDiameter = Float.parseFloat(pp.getProperty(PilotProps.KEY_WHEELDIAMETER, "" + WHEEL_DIAMETER)); //was "4.96"
		    	float trackWidth = Float.parseFloat(pp.getProperty(PilotProps.KEY_TRACKWIDTH, "" + WHEEL_DISTANCE));//was "13.0"
		    	RegulatedMotor leftMotor = PilotProps.getMotor(pp.getProperty(PilotProps.KEY_LEFTMOTOR, "B"));
		    	RegulatedMotor rightMotor = PilotProps.getMotor(pp.getProperty(PilotProps.KEY_RIGHTMOTOR, "C"));
		    	boolean reverse = Boolean.parseBoolean(pp.getProperty(PilotProps.KEY_REVERSE,"true"));
		    
		    	boolean backReverse = Boolean.parseBoolean(pp.getProperty(PilotProps.KEY_REVERSE,"false"));
		    	
		    	DifferentialPilot pilot = new DifferentialPilot(WHEEL_DIAMETER, WHEEL_DISTANCE, leftMotor, rightMotor, true);
		    	backPilot = new DifferentialPilot(WHEEL_DIAMETER, WHEEL_DISTANCE, leftMotor, rightMotor, false);

				//Navigator nav = new Navigator(pilot);
				forwardNav = new BTNavigator(pilot, FORWARD);
				forwardNav.move();
		    
//				
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			
		}
	}
	
	public void move() throws IOException {
	
		boolean running = true;
		
		do {
			int commandType = (int) dataInputStream.readFloat();
			CommandType command = CommandType.values()[commandType];
			
			LCD.clear();
			onLCDPositionY = 1;
			LCD.drawString(command.toString(), 0, onLCDPositionY++);
			LCD.drawInt(commandType, 0, onLCDPositionY++);
		
			if(command == CommandType.MOVE) {
				moveToDestination2();
			}
			else if(command == CommandType.CLAW_UP) {
				moveClawUp();
			}
			else if(command == CommandType.CLAW_DOWN) {
				moveClawDown();
			}
			else if(command == CommandType.RESET) {
				break;
			}
			else if(command == CommandType.BACK) {
				new BTNavigatorBackwards(backPilot).moveBack();
			}
			else if(command == CommandType.LOCATION) {
				sendMyLocation();
			}
			else if(command == CommandType.END) {
				dataInputStream.close();
				dataOutputStream.close();
				btc.close();
				running = false;
				
			}		
			
		}while(running);
		if(running)
			setRobotSpecs();
	}

	private void sendMyLocation() {
		Pose currentPose = poseProviderForward.getPose();
		Point location = currentPose.getLocation();
		
		LCD.drawString("Sending my location", 0, onLCDPositionY++);
		
		try {
			dataOutputStream.writeFloat(location.x);
			//dataOutputStream.writeFloat(location.y);
			LCD.refresh();
			LCD.drawString("x: " + location.x, 0, onLCDPositionY++);
			//LCD.drawString("y: " + location.y, 0, onLCDPositionY++);

		} catch (IOException e) {
			// TODO Auto-generated catch block
			System.out.println("IO Exception Reading Bytes: ");
			e.printStackTrace();
		}
	}
	
	private void moveClawDown() {
		// TODO Auto-generated method stub
		Motor.A.setSpeed(45);
		Motor.A.rotate(90);
	}

	private void moveClawUp() {
		// TODO Auto-generated method stub
		Motor.A.setSpeed(45);
		Motor.A.rotate(-90);
		
	}
	
	public void setRobotSpecs(){
		LCD.drawString("Setting Robot Specs", 0, onLCDPositionY++);
		
	//	pose = poseProviderForward.getPose();
		try {
		 	
			PilotProps pp = new PilotProps();
			
			pp.setProperty(PilotProps.KEY_WHEELDIAMETER, "" + WHEEL_DIAMETER);
			pp.setProperty(PilotProps.KEY_TRACKWIDTH, "" + WHEEL_DISTANCE);
			
	    	pp.loadPersistentValues();
	    	float wheelDiameter = Float.parseFloat(pp.getProperty(PilotProps.KEY_WHEELDIAMETER, "" + WHEEL_DIAMETER));//was "4.96"
	    	float trackWidth = Float.parseFloat(pp.getProperty(PilotProps.KEY_TRACKWIDTH, "" + WHEEL_DISTANCE));//was "13.0"
	    	RegulatedMotor leftMotor = PilotProps.getMotor(pp.getProperty(PilotProps.KEY_LEFTMOTOR, "B"));
	    	RegulatedMotor rightMotor = PilotProps.getMotor(pp.getProperty(PilotProps.KEY_RIGHTMOTOR, "C"));
	    	boolean reverse = Boolean.parseBoolean(pp.getProperty(PilotProps.KEY_REVERSE,"true"));
	    
	    	boolean backReverse = Boolean.parseBoolean(pp.getProperty(PilotProps.KEY_REVERSE,"false"));
	    	
	    	DifferentialPilot pilot = new DifferentialPilot(WHEEL_DIAMETER, WHEEL_DISTANCE, leftMotor, rightMotor, reverse);
	    	backPilot = new DifferentialPilot(WHEEL_DIAMETER, WHEEL_DISTANCE, leftMotor, rightMotor, backReverse);

			//Navigator nav = new Navigator(pilot);
			forwardNav = new BTNavigator(pilot, FORWARD);
			forwardNav.move();
	    
//			
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
	}
	
	
	private void moveToDestination2() throws IOException{
		float x = dataInputStream.readFloat();
		float z = dataInputStream.readFloat();
		
		LCD.drawString("x: "+x, 0, onLCDPositionY++);
		LCD.refresh();
		LCD.drawString("z: "+z, 0, onLCDPositionY++);
		LCD.refresh();
		LCD.drawString("Me: "+(int)poseProviderForward.getPose().getX()+" , "+(int)poseProviderForward.getPose().getY(), 0, onLCDPositionY++);
		LCD.refresh();
		LCD.drawString("Rot: "+poseProviderForward.getPose().getHeading(), 0, onLCDPositionY++);
		LCD.refresh();
		
		Point destination = new Point(x,z);
		
		float result = poseProviderForward.getPose().relativeBearing(destination);
		LCD.drawString("Angle: "+result, 0, onLCDPositionY++);
		classPilotForward.rotate(result);
		LCD.drawString("Distance: "+poseProviderForward.getPose().distanceTo(destination), 0, onLCDPositionY++);
		classPilotForward.travel(poseProviderForward.getPose().distanceTo(destination));
	}
	
	private void moveToDestination() throws IOException {
		// TODO Auto-generated method stub
		float x = dataInputStream.readFloat();
		float z = dataInputStream.readFloat();
		
		LCD.drawString("x: "+x, 0, onLCDPositionY++);
		LCD.refresh();
		LCD.drawString("z: "+z, 0, onLCDPositionY++);
		LCD.refresh();
		
	
		Pose pose = poseProviderForward.getPose();
		Point destination = new Point(x,z);
		float angle = pose.angleTo(destination);
		
		
		LCD.drawInt((int) pose.getHeading(), 0, onLCDPositionY++);
		
		float angleToRotate = angle - pose.getHeading();
		
		LCD.drawString("To rotate, angle : " + angleToRotate, 0, onLCDPositionY++);
		LCD.drawInt( (int) ((int)angle - pose.getHeading()), 0, onLCDPositionY++);
		
		
		classPilotForward.rotate(angleToRotate);
		classPilotForward.travel(pose.distanceTo(destination));
		
	}
	
	public static void connectTheDevice() {
		LCD.drawString("This is Speared!", 0, onLCDPositionY++);
		LCD.refresh();
		
		btc = Bluetooth.waitForConnection(0, NXTConnection.RAW);
		
		dataInputStream = btc.openDataInputStream();
		dataOutputStream  = btc.openDataOutputStream();
	}
}

// NAVIGATOR TO DRIVE THE ROBOT BACKWARDS

class BTNavigatorBackwards{
	private ArcRotateMoveController classPilotBackward;
	private OdometryPoseProvider poseProviderBackward;

	private Pose pose = new Pose();

	
	public BTNavigatorBackwards(final ArcRotateMoveController pilot) {
		classPilotBackward = pilot;
		poseProviderBackward = new OdometryPoseProvider(classPilotBackward);
		classPilotBackward.setTravelSpeed(25);
		classPilotBackward.setRotateSpeed(90);
	}
	
	public void moveBack() {
		try {
			float x = BTNavigator.dataInputStream.readFloat();
			float z = BTNavigator.dataInputStream.readFloat();
			

			LCD.drawString("BACK MOVE " + x + " " + z, 0, BTNavigator.onLCDPositionY++);
			LCD.refresh();
			
			pose = poseProviderBackward.getPose();		
			
			LCD.drawString("MOVE TO " + x + " " + z, 0, BTNavigator.onLCDPositionY++);
			LCD.refresh();
			
			
			Point destination = new Point(x,z);
			
			classPilotBackward.travel(pose.distanceTo(destination));
			
			BTNavigator.forwardNav.move();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}	
	}
}
