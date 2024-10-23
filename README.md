# StockTrack

**StockTrack** is an Angular and .NET application designed for efficient inventory management.

## Installation Instructions

1. **Clone the Git repository** using your terminal:

    ```bash
    git clone https://github.com/HevgerExternal/StockTrack.git
    ```

2. **Navigate to the StockTrack directory**:

    ```bash
    cd StockTrack
    ```

3. **Ensure you have Docker and Docker Compose** installed on your machine, then run the following command to build and start the application:

    ```bash
    docker-compose up --build -d
    ```

4. **Install 'mkcert'** on your computer, which you can obtain from [here](https://github.com/FiloSottile/mkcert). After installation, set up the local Certificate Authority using:

    ```bash
    mkcert -install
    ```

5. **Create the certificate and key files** to replace the existing certificates. Change to the 'Certificates' directory and run the following command:

    ```bash
    cd Certificates
    mkcert -key-file stocktrack.local.key -cert-file stocktrack.local.crt app.stocktrack.local api.stocktrack.local
    ```

6. **Add the following entries to your hosts file**. Refer to this [guide](https://www.hostinger.com/tutorials/how-to-edit-hosts-file) for assistance:

    ```plaintext
    127.0.0.1 app.stocktrack.local api.stocktrack.local
    ```

7. You should now be able to access the application at **https://app.stocktrack.local**.