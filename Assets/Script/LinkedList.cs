using System.Collections;

public class LinkedList<DataType>
{
	public int count;
	private Node<DataType> head;

	public LinkedList(){
		head = null;
		count = 0;
	}

	public int find(DataType node){
		Node<DataType> temp = head;
		int n = 0;
		while (temp!=null) {
			if (temp.data.Equals(node)) {
				return n;
			}
			temp = temp.next;
			n++;
		}
		return -1;
	}
	public DataType pop(){
		Node<DataType> temp = head;
		if(head==null)
			return default(DataType);
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
        if (head == null) {
            return default(DataType);
        }
		while (n != index&&temp.next!=null) {
			temp = temp.next;
		}
		if (temp != null)
			return temp.data;
		else
			return default(DataType);
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

