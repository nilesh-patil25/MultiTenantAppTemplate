// eslint-disable-next-line no-unused-vars
import React, { useState } from "react";
import axios from "axios";

const Home = () => {
  // State to store image data
  const [imageData, setImageData] = useState(null);

  // Function to fetch image data
  const fetchImageData = async () => {
    try {
      // Make a GET request to the backend API endpoint
      const response = await axios.get(
        "https://localhost:7213/api/Image/downloadFaviconImage?hostName=foo",
        {
          responseType: "blob", // Specify responseType as 'blob' to receive binary data
        }
      );

      // Read the response data as a blob
      const blob = await response.data;

      // Convert the blob to a base64 string
      const reader = new FileReader();
      reader.readAsDataURL(blob);
      reader.onloadend = () => {
        const base64data = reader.result;

        // Set the base64 image data to the state
        setImageData(base64data);
      };
    } catch (error) {
      console.error("Error fetching image data:", error);
    }
  };

  // Function to handle file change
  const handleFileChange = (event) => {
    const file = event.target.files[0];
    uploadImage(file);
  };

  // Function to upload image
  const uploadImage = async (file) => {
    try {
      const formData = new FormData();
      formData.append("file", file);

      // Make a POST request to upload the image
      await axios.post(
        "https://localhost:7213/api/Image/foo/banner",
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );

      alert("Image uploaded successfully!");
    } catch (error) {
      console.error("Error uploading image:", error);
      alert("Error uploading image. Please try again.");
    }
  };

  return (
    <div>
      <h1>Home Page</h1>
      <input type="file" onChange={handleFileChange} />
      {/* Button to fetch and display the image */}
      <button onClick={fetchImageData}>Download Image</button>
      {/* Display the image if imageData is not null */}
      {imageData && <img src={imageData} alt="Banner" />}
    </div>
  );
};

export default Home;
