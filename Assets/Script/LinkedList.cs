using System.Collections;

public class LinkedList<DataType>
{
	public int count;
	private Node<DataType> head;

	public LinkedList(){
		head = null;
		count = 0;
	}

	public DataType pop(){
		Node<DataType> temp = head;
		head = head.next;
		count--;
		return temp.data;
	}

	public void add(DataType node){
		if (head == null) {
			head = new Node<DataType> (node);
		} else {
			Node<DataType> tempNode = head;
			while(tempNode.next!=null)
				tempNode = tempNode.next;
			tempNode.next = new Node<DataType> (node);
		}
		count++;
	}
	public DataType get(int index){
		Node<DataType> temp = head;
		int n = 0;
		while (n != index&&temp.next!=null) {
			temp = temp.next;
		}
		return temp.data;
	}

	class Node<NodeDataType>{
		public NodeDataType data;
		public Node<NodeDataType> next;

		public Node(NodeDataType data){
			this.data = data;
			next = null;
		}
	}
}

