import React from "react";

const Dashboard = () => {
  return (
    <div className="flex flex-col items-center justify-center min-h-screen py-8">
      <h2 className="text-3xl font-semibold mb-4">Welcome to Your Dashboard</h2>

      {/* Quick Stats */}
      <div className="w-full max-w-3xl grid grid-cols-1 md:grid-cols-3 gap-4 mb-8">
        <div className="bg-white p-6 rounded-lg shadow-md">
          <h3 className="text-lg font-semibold mb-2">Total Users</h3>
          <p className="text-xl">1000</p>
        </div>
        <div className="bg-white p-6 rounded-lg shadow-md">
          <h3 className="text-lg font-semibold mb-2">Total Orders</h3>
          <p className="text-xl">500</p>
        </div>
        <div className="bg-white p-6 rounded-lg shadow-md">
          <h3 className="text-lg font-semibold mb-2">Revenue</h3>
          <p className="text-xl">$10,000</p>
        </div>
      </div>

      {/* Recent Orders */}
      <div className="w-full max-w-3xl bg-white rounded-lg shadow-md">
        <h3 className="text-lg font-semibold bg-gray-200 px-6 py-4 border-b">
          Recent Orders
        </h3>
        <div className="p-6">
          <ul>
            <li className="flex justify-between items-center py-2 border-b">
              <span>Order #123</span>
              <span>$100</span>
            </li>
            <li className="flex justify-between items-center py-2 border-b">
              <span>Order #124</span>
              <span>$150</span>
            </li>
            <li className="flex justify-between items-center py-2 border-b">
              <span>Order #125</span>
              <span>$200</span>
            </li>
          </ul>
        </div>
      </div>

      {/* Recent Activity */}
      <div className="w-full max-w-3xl bg-white rounded-lg shadow-md mt-8">
        <h3 className="text-lg font-semibold bg-gray-200 px-6 py-4 border-b">
          Recent Activity
        </h3>
        <div className="p-6">
          <ul>
            <li className="flex justify-between items-center py-2 border-b">
              <span>User X liked your post</span>
              <span>2 hours ago</span>
            </li>
            <li className="flex justify-between items-center py-2 border-b">
              <span>User Y commented on your photo</span>
              <span>3 hours ago</span>
            </li>
            <li className="flex justify-between items-center py-2 border-b">
              <span>User Z sent you a message</span>
              <span>4 hours ago</span>
            </li>
          </ul>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
