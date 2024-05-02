// eslint-disable-next-line no-unused-vars
import React, { useState, useEffect } from "react";
import axios from "axios";

const Foo = () => {
  const [assetImages, setAssetImages] = useState([]);

  useEffect(() => {
    fetchAssetImages();
  }, []);

  const fetchAssetImages = async () => {
    try {
      const response = await axios.get("/assets/foo");
      setAssetImages(response.data);
    } catch (error) {
      console.error("Error fetching asset images:", error);
    }
  };

  const handleFileChange = (event) => {
    const file = event.target.files[0];
    uploadImage(file);
  };

  const uploadImage = async (file) => {
    try {
      const formData = new FormData();
      formData.append("file", file);

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

  const handleButtonClick = () => {
    const fileInput = document.getElementById("file-input");
    fileInput.click();
  };

  return (
    <>
      <div>
        <h1>Tenant || Foo</h1>
        <button onClick={handleButtonClick}>Upload Image</button>
        <input
          id="file-input"
          type="file"
          onChange={handleFileChange}
          style={{ display: "none" }}
        />
        {""}
        {/* <button onClick={fetchImageData}>Download Image</button>
        {imageData && <img src={imageData} alt="Banner" />} */}

        <div style={{ display: "flex", flexWrap: "wrap" }}>
          {assetImages.map((image, index) => (
            <img
              key={index}
              style={{ width: "200px", height: "200px", margin: "12px" }}
              src={`assets/foo/${image}`}
              alt={`Image ${index}`}
            />
          ))}
        </div>
      </div>
    </>
  );
};

export default Foo;
